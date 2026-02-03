using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool CanMove => _canMove;
    
    [SerializeField] private float _movementSpeed = 10;
    
    private Rigidbody2D _rigidBody;
    private float _moveX;
    private bool _canMove = true;
    private Knockback _knockback;
    

    public void SetCurrentDirection(float currentDirection)
    {
        _moveX = currentDirection;
    }
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
        _knockback = GetComponent<Knockback>();
    }

    private void OnEnable()
    {
        _knockback.OnKnockbackStart += CanMoveFalse;
        _knockback.OnKnockbackEnd += CanMoveTrue;
    }
    private void OnDisable()
    {
        _knockback.OnKnockbackStart -= CanMoveFalse;
        _knockback.OnKnockbackEnd -= CanMoveTrue;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void CanMoveFalse()
    {
        _canMove = false;
    }
    private void CanMoveTrue()
    {
        _canMove = true;
    }
    private void Move()
    {
        if (!_canMove) return;
        
        var movement = new Vector2(_moveX * _movementSpeed, _rigidBody.linearVelocityY);
        _rigidBody.linearVelocity = movement;
    }
    
}
