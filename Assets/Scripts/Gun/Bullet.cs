using System.Collections;
using System.Collections.Generic;
using Combat;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject _bulletParticlesPrefab;
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private int _damageAmount = 1;
    [SerializeField] private float _knockbackThrust = 20;

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
        
        Instantiate(_bulletParticlesPrefab, transform.position, Quaternion.identity);

        if (other.TryGetComponent(out IHitable hitable))
            hitable.TakeHit();

        if (other.TryGetComponent(out IDamageable damageable)) 
            damageable.TakeDamage(_damageAmount, _knockbackThrust);
        
        
        _gun.ReleaseBulletPool(this);
    }
}