using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class Gun : MonoBehaviour
{
    private static readonly int FIRE_HASH = Animator.StringToHash("Fire");
    
    public static Action OnShot;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;
    [SerializeField] private float _gunFireCD = 0.5f;

    private float _lastFireTime = 0f;
    private Vector2 _mousePosition;
    private Animator _animator;
    
    private ObjectPool<Bullet> _bulletPool;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        CreateBulletPool();
    }

    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void OnEnable()
    {
        OnShot += ShootProjectile;
        OnShot += ResetLastFireTime;
        OnShot += FireAnimation;
    }

    private void OnDisable()
    {
        OnShot -= ShootProjectile;
        OnShot -= ResetLastFireTime;
        OnShot -= FireAnimation;
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
        if (Input.GetMouseButton(0) && Time.time >= _lastFireTime) {
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

    private void RotateGun()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePosition); // takes the local position, so if player is flip, still gets the right direction
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // angle between -180 and 180 degrees
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
    
    public void ReleaseBulletPool(Bullet bullet) => _bulletPool.Release(bullet);
}
