using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;

    private Vector2 _fireDirection;

    private Rigidbody2D _rigidBody;
    private Gun _gun;

    public void Init(Gun gun,Vector2 bulletSpawnPosition, Vector2 mousePosition)
    {
        _gun = gun;
        transform.SetPositionAndRotation(bulletSpawnPosition, Quaternion.identity);
        _fireDirection = (mousePosition - bulletSpawnPosition).normalized;
    }
    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        _rigidBody.linearVelocity = _fireDirection * _moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        Health health = other.gameObject.GetComponent<Health>();
        health?.TakeDamage(_damageAmount);
        _gun.ReleaseBulletPool(this);
    }
}