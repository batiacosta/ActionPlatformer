using System;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 10;
    
    private Rigidbody2D _rigidBody;
    private float _moveX;

    public void SetCurrentDirection(float currentDirection)
    {
        _moveX = currentDirection;
    }
    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        var movement = new Vector2(_moveX * _movementSpeed, _rigidBody.linearVelocityY);
        _rigidBody.linearVelocity = movement;
    }
}
