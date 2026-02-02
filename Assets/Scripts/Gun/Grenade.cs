using System;
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

    [SerializeField] private float _launchForce = 15f;
    [SerializeField] private float _torqueAmount = 2f;

    private Rigidbody2D _rigidbody;
    private CinemachineImpulseSource _impulseSource;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Start()
    {
        LaunchGrenade();
    }

    private void LaunchGrenade()
    {
        var mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
        var directionToMouse = (mousePos - transform.position).normalized;
        _rigidbody.AddForce(directionToMouse * _launchForce, ForceMode2D.Impulse);
        _rigidbody.AddTorque(_torqueAmount, ForceMode2D.Impulse);
    }
}
