using System;
using System.Collections;
using UnityEngine;

public class Knockback : MonoBehaviour
{
    public Action OnKnockbackStart;
    public Action OnKnockbackEnd;
    
    [SerializeField] private float _knockbackTime = 0.2f;
    
    private Rigidbody2D _rigidbody;
    private float _knockbackThrust;
    private Vector3 _hitDirection;
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        OnKnockbackStart += ApplyKnockbackForce;
        OnKnockbackEnd += StopKnockback;
    }

    private void OnDisable()
    {
        OnKnockbackStart -= ApplyKnockbackForce;
        OnKnockbackEnd -= StopKnockback;
    }

    public void GetKnockedBack(Vector3 hitDirection, float knockbackThrust)
    {
        _hitDirection = hitDirection;
        _knockbackThrust = knockbackThrust;
        
        OnKnockbackStart?.Invoke();
    }

    private void ApplyKnockbackForce()
    {
        var difference = (transform.position - _hitDirection).normalized * _knockbackThrust * _rigidbody.mass;
        _rigidbody.AddForce(difference, ForceMode2D.Impulse);

        StartCoroutine(KnockRoutine());
    }

    private IEnumerator KnockRoutine()
    {
        yield return new WaitForSeconds(_knockbackTime);
        OnKnockbackEnd?.Invoke();
    }

    private void StopKnockback()
    {
        _rigidbody.linearVelocity = Vector2.zero;
    }
}
