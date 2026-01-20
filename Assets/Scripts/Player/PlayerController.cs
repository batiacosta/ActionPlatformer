using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;
    public static Action OnJump;
    public static Action OnJetpack;
    
    public Vector2 MoveInput => _frameInput.Move;

    [SerializeField] private TrailRenderer _jetpackTrailRenderer;
    [SerializeField] private float _jumpStrength = 7f;
    [SerializeField] private Transform _feetTransform;
    [SerializeField] private Vector2 _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _extraGravity = 700f;
    [SerializeField] private float _gravityDelay = 0.2f;
    [SerializeField] private float _coyoteTime = 0.5f;
    [SerializeField] private float _jetpackTime = 0.6f;
    [SerializeField] private float _jetpackStrength = 11f;
    
    private Rigidbody2D _rigidBody;

    private PlayerInput _playerInput;  
    private FrameInput _frameInput;
    private Movement _movement;
    private float _timeInAir, _coyoteTimer;
    private bool _doubleJumpAvailable;
    private Coroutine _jetpackCoroutine;

    public void Awake() {
        if (Instance == null) { Instance = this; }

        _rigidBody = GetComponent<Rigidbody2D>();
        _playerInput = GetComponent<PlayerInput>();
        _movement = GetComponent<Movement>();
    }

    private void OnEnable()
    {
        OnJump += ApplyJumpForce;
        OnJetpack += StartJetpack;
    }

    private void OnDisable()
    {
        OnJump -= ApplyJumpForce;
        OnJetpack -= StartJetpack;
    }
    public bool CheckIfGrounded()
    {
        var isGrounded = Physics2D.OverlapBox(_feetTransform.position, _groundCheck, 0f, _groundLayer);
        return isGrounded;
    }

    private void Update()
    {
        GatherInput();
        Movement();
        HandleJump();
        CoyoteTimer();
        HandleSpriteFlip();
        GravityDelay();
        Jetpack();
    }

    private void FixedUpdate()
    {
        ApplyExtraGravity();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(_feetTransform.position, _groundCheck);
    }

    public bool IsFacingRight()
    {
        return transform.eulerAngles.y == 0;
    }

    private void GravityDelay()
    {
        if (!CheckIfGrounded())
        {
            _timeInAir += Time.deltaTime;
        }
        else
        {
            _timeInAir = 0f;
        }
    }

    private void ApplyExtraGravity()
    {
        if (_timeInAir > _gravityDelay)
        {
            _rigidBody.AddForce(new Vector2(0, -_extraGravity * Time.deltaTime));
        }
    }
    private void GatherInput()
    {
        _frameInput = _playerInput.FrameInput;
    }

    private void Movement() {
        _movement.SetCurrentDirection(_frameInput.Move.x);
    }

    private void HandleJump()
    {
        if (!_frameInput.Jump) return;
        
        if (CheckIfGrounded())
        {
            OnJump?.Invoke();
        } else if (_coyoteTimer > 0f)
        {
            OnJump?.Invoke();
        } else if (_doubleJumpAvailable) 
        {
            _doubleJumpAvailable = false;
            OnJump?.Invoke();
        }
    }
    private void CoyoteTimer()
    {
        if (CheckIfGrounded())
        {
            _coyoteTimer = _coyoteTime;
            _doubleJumpAvailable = true;
        }
        else
        {
            _coyoteTimer -= Time.deltaTime;
        }
    }

    private void ApplyJumpForce()
    {
        _rigidBody.linearVelocityY = 0;
        _timeInAir = 0f;
        _coyoteTimer = 0f;
        _rigidBody.AddForce(Vector2.up * _jumpStrength, ForceMode2D.Impulse);
    }

    private void HandleSpriteFlip()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());

        if (mousePosition.x < transform.position.x)
        {
            transform.eulerAngles = new Vector3(0f, -180f, 0f);
        }
        else
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
    }

    private void Jetpack()
    {
        if(!_frameInput.Jetpack || _jetpackCoroutine != null) return;
        
        OnJetpack?.Invoke();
    }

    private void StartJetpack()
    {
        _jetpackCoroutine = StartCoroutine(JetpackRoutine());
    }

    private IEnumerator JetpackRoutine()
    {
        var jetTime = 0f;

        while (jetTime < _jetpackTime)
        {
            jetTime += Time.deltaTime;
            _rigidBody.linearVelocity = Vector2.up * _jetpackStrength;
            yield return null;
        }

        _jetpackCoroutine = null;
    }
}
