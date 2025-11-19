 using System;
 using Unity.Cinemachine;
 using Unity.VisualScripting;
 using UnityEngine;

 public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustParticles;
    [SerializeField] private float _tiltAngle = 20;
    [SerializeField] private float _tiltSpeed = 5f;
    [SerializeField] private float _hatTiltingModifier = 5;
    [SerializeField] private Transform _characterSpriteTransform;
    [SerializeField] private Transform _hatSpriteTransform;
    [SerializeField] private ParticleSystem _poofParticles;
    [SerializeField] private float _yVelocityCheck = -10;

    private Vector2 _velocityBeforePhysicsUpdate;
    private Rigidbody2D _rigidbody;
    private CinemachineImpulseSource _cinemachineImpulseSource;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void OnEnable()
    {
        PlayerController.OnJump += PlayPoofDustEffects;
    }

    private void OnDisable()
    {
        PlayerController.OnJump -= PlayPoofDustEffects;
    }

    private void Update()
    {
        DetectMoveDust();
        ApplyTilting();
    }

    private void FixedUpdate()
    {
        _velocityBeforePhysicsUpdate = _rigidbody.linearVelocity  ;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (_velocityBeforePhysicsUpdate.y < _yVelocityCheck)
        {
            PlayPoofDustEffects();
            _cinemachineImpulseSource.GenerateImpulse();
        }
    }

    private void DetectMoveDust()
    {
        if (PlayerController.Instance.CheckIfGrounded())
        {
            if(!_moveDustParticles.isPlaying) _moveDustParticles.Play();
        }
        else
        {
            if (_moveDustParticles.isPlaying) _moveDustParticles.Stop();
        }
    }

    private void ApplyTilting()
    {
        float tiltingAngle;
        if (PlayerController.Instance.MoveInput.x < 0)
        {
            tiltingAngle = _tiltAngle;
        } else if (PlayerController.Instance.MoveInput.x > 0)
        {
            tiltingAngle = -_tiltAngle;
        } else{
            tiltingAngle = 0;
        }
        
        var currentPlayerRotation = _characterSpriteTransform.rotation;
        var targetCharacterRotation = Quaternion.Euler(
            currentPlayerRotation.eulerAngles.x,
            currentPlayerRotation.eulerAngles.y,
            tiltingAngle
        );
        _characterSpriteTransform.rotation = Quaternion.Lerp(currentPlayerRotation, targetCharacterRotation, _tiltSpeed * Time.deltaTime);

        var currentHatRotation = _hatSpriteTransform.rotation;
        var targetHatRotation = Quaternion.Euler(
                currentHatRotation.eulerAngles.x,
                currentHatRotation.eulerAngles.y,
                - tiltingAngle / _hatTiltingModifier
            );
        _hatSpriteTransform.rotation = Quaternion.Lerp(currentHatRotation, targetHatRotation, _tiltSpeed * _hatTiltingModifier * Time.deltaTime);
    }

    private void PlayPoofDustEffects()
    {
        _poofParticles.Play();
    }
}
