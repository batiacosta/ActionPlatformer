 using System;
 using UnityEngine;
 using UnityEngine.Rendering;

 public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustParticles;
    [SerializeField] private float _tiltAngle = 20;
    [SerializeField] private float _tiltSpeed = 5f;
    [SerializeField] private Transform _characterSpriteTransform;

    private void Update()
    {
        DetectMoveDust();
        ApplyTilting();
    }

    private void DetectMoveDust()
    {
        if (PlayerController.Instance.CheckIfGrounded())
        {
            if(!_moveDustParticles.isPlaying) _moveDustParticles.Play();
        }
        else
        {
            if (_moveDustParticles.isPlaying) _moveDustParticles.Stop();
        }
    }

    private void ApplyTilting()
    {
        float tiltingAngle;
        if (PlayerController.Instance.MoveInput.x < 0)
        {
            tiltingAngle = _tiltAngle;
        } else if (PlayerController.Instance.MoveInput.x > 0)
        {
            tiltingAngle = -_tiltAngle;
        } else{
            tiltingAngle = 0;
        }
        
        var currentPlayerRotation = _characterSpriteTransform.rotation;
        var targetCharacterRotation = Quaternion.Euler(
            currentPlayerRotation.eulerAngles.x,
            currentPlayerRotation.eulerAngles.y,
            tiltingAngle
        );
        _characterSpriteTransform.rotation = Quaternion.Lerp(currentPlayerRotation, targetCharacterRotation, _tiltSpeed * Time.deltaTime);
    }
}
