using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grenade : MonoBehaviour
{
    // Rig up Player Input to launch it with Right click
    // Lob grenade and spin  it with RigidBody2D.AddTorque
    // Beeb 3 times before detonation
    // Create explosion
    // Use Physics2D.OverlapCircleAll to damage all enemies it hits

    public Action OnExplode;
    public Action OnBeep;
    
    [SerializeField] private float _launchForce = 15f;
    [SerializeField] private float _torqueAmount = 2f;
    [SerializeField] private float _explosionRadious = 3.5f;
    [SerializeField] private int _damageAmount = 3;
    [SerializeField] private float _lightBlinkTime = 0.15f;
    [SerializeField] private int _totalBlinks = 3;
    [SerializeField] private float _explodeTime = 3f;
    [SerializeField] private GameObject _explodeVFX;
    [SerializeField] private GameObject _grenadeLight;
    [SerializeField] private LayerMask _enemyLayerMask;

    private Rigidbody2D _rigidbody;
    private CinemachineImpulseSource _impulseSource;
    private int _currentBlinks = 0;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        LaunchGrenade();
        StartCoroutine(CountdownExplodeRoutine());
    }

    private void OnEnable()
    {
        OnExplode += Explosion;
        OnExplode += GrenadeScreenShake;
        OnExplode += DamageNearby;
        OnBeep += BlinkLight;
    }

    private void OnDisable()
    {
        OnExplode -= Explosion;
        OnExplode -= GrenadeScreenShake;
        OnExplode -=DamageNearby;
        OnBeep -= BlinkLight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.TryGetComponent(out Enemy enemy)) return;
        
        OnExplode?.Invoke();
    }

    private void LaunchGrenade()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var directionToMouse = (mousePos - transform.position).normalized;
        _rigidbody.AddForce(directionToMouse * _launchForce, ForceMode2D.Impulse);
        _rigidbody.AddTorque(_torqueAmount, ForceMode2D.Impulse);
    }

    private void Explosion()
    {
        Instantiate(_explodeVFX, transform.position, Quaternion.identity);
        
        Destroy(gameObject);
    }

    private void GrenadeScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void DamageNearby()
    {
        var hits = Physics2D.OverlapCircleAll(transform.position, _explosionRadious, _enemyLayerMask);

        foreach (var hit in hits)
        {
            var health = hit.GetComponent<Health>();
            health?.TakeDamage(_damageAmount);
        }
    }
    
    private IEnumerator CountdownExplodeRoutine(){
        while (_currentBlinks < _totalBlinks)
        {
            yield return new WaitForSeconds(_explodeTime / _totalBlinks);
            OnBeep?.Invoke();
            yield return new WaitForSeconds(_lightBlinkTime);
            _grenadeLight.gameObject.SetActive(false);
        }
        _currentBlinks = 0;
        OnExplode?.Invoke();
    }

    private void BlinkLight()
    {
        _grenadeLight.gameObject.SetActive(true);
        _currentBlinks++;
    }
}
