using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class OneWayPlatform : MonoBehaviour
{
    [SerializeField] private float _disableColliderTime = 1;
    
    private bool _playerOnPlatform = false;
    private Collider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        DetectPlayerInput();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            _playerOnPlatform = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.GetComponent<PlayerController>())
        {
            _playerOnPlatform = false;
        }
    }
    
    private void DetectPlayerInput()
    {
        if (!_playerOnPlatform) return;

        if (PlayerController.Instance.MoveInput.y < 0)
        {
            StartCoroutine(DisablePlatformColliderRoutine());
        }
    }

    private IEnumerator DisablePlatformColliderRoutine()
    {
        var playerColliders = PlayerController.Instance.GetComponents<Collider2D>();

        foreach (var playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, _collider, true);
        }
        
        yield return new WaitForSeconds(_disableColliderTime);
        
        foreach (var playerCollider in playerColliders)
        {
            Physics2D.IgnoreCollision(playerCollider, _collider, false);
        }
    }
}
