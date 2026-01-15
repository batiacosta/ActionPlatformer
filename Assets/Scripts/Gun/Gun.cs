using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.InputSystem;

public class Gun : MonoBehaviour
{
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    
    public static Action OnShot;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCD = 0.5f;
    [SerializeField] private GameObject _muzzleFlash;
    [SerializeField] private float _muzzleFlashTime = 0.05f;

    private float _lastFireTime = 0f;
    private Vector2 _mousePosition;
    private Animator _animator;
    private CinemachineImpulseSource _impulseSource;
    private Coroutine _muzzleFlashCoroutine;
    
    private ObjectPool<Bullet> _bulletPool;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _impulseSource = GetComponent< CinemachineImpulseSource>();
        CreateBulletPool();
    }
    
    private void OnEnable()
    {
        PlayerInput.OnShot += Shoot;
        OnShot += ShootProjectile;
        OnShot += ResetLastFireTime;
        OnShot += FireAnimation;
        OnShot += GunScreenShake;
        OnShot += MuzzleFlash;
    }

    private void OnDisable()
    {
        PlayerInput.OnShot -= Shoot;
        OnShot -= ShootProjectile;
        OnShot -= ResetLastFireTime;
        OnShot -= FireAnimation;
        OnShot -= GunScreenShake;
        OnShot -= MuzzleFlash;
    }
    
    public void ReleaseBulletPool(Bullet bullet) => _bulletPool.Release(bullet);
    
    private void Update()
    {
        //Shoot();
        RotateGun();
    }
    
    private void CreateBulletPool()
    {
        _bulletPool = new ObjectPool<Bullet>(
            () => Instantiate(_bulletPrefab),
            bullet => { bullet.gameObject.SetActive(true);},
            bullet => { bullet.gameObject.SetActive(false);},
            bullet => { Destroy(bullet.gameObject);},
            false, // collection checking is not needed here and saves CPU
            20,
            40
        );
    }
    private void Shoot()
    {
        if ( Time.time >= _lastFireTime) {
            OnShot?.Invoke();
        }
    }

    private void ShootProjectile()
    {
        Bullet newBullet = _bulletPool.Get();
        newBullet.Init(this, bulletSpawnPosition:  _bulletSpawnPoint.position, mousePosition: _mousePosition);
    }

    private void ResetLastFireTime()
    {
        _lastFireTime = Time.time + _gunFireCD;
    }

    private void FireAnimation()
    {
        _animator.Play(FIRE_HASH, 0, 0f);
    }

    private void GunScreenShake()
    {
        _impulseSource.GenerateImpulse();
    }

    private void RotateGun()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePosition); // takes the local position, so if player is flip, still gets the right direction
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // angle between -180 and 180 degrees
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    private void MuzzleFlash()
    {
        _muzzleFlash.SetActive(true);
        if (_muzzleFlashCoroutine != null)
        {
            StopCoroutine(_muzzleFlashCoroutine);
        }

        _muzzleFlashCoroutine = StartCoroutine(MuzzleFlashRouine());
    }

    private IEnumerator MuzzleFlashRouine()
    {
        yield return new WaitForSeconds(_muzzleFlashTime);
        _muzzleFlash.SetActive(false);
    }
}
