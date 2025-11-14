using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public Transform BulletSpawnPoint => _bulletSpawnPoint;

    [SerializeField] private Transform _bulletSpawnPoint;
    [SerializeField] private Bullet _bulletPrefab;

    private Vector2 _mousePosition;
    private void Update()
    {
        Shoot();
        RotateGun();
    }

    private void Shoot()
    {
        if (Input.GetMouseButtonDown(0)) {
            ShootProjectile();
        }
    }

    private void ShootProjectile()
    {
        Bullet newBullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, Quaternion.identity);
        newBullet.Init(bulletSpawnPosition:  _bulletSpawnPoint.position, mousePosition: _mousePosition);
    }

    private void RotateGun()
    {
        _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var direction = PlayerController.Instance.transform.InverseTransformPoint(_mousePosition); // takes the local position, so if player is flip, still gets the right direction
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; // angle between -180 and 180 degrees
        transform.localRotation = Quaternion.Euler(0, 0, angle);
    }
}
