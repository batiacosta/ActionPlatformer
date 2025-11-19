 using System;
 using UnityEngine;

 public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustParticles;
    [SerializeField] private float _tiltAngle = 20;
    [SerializeField] private float _tiltSpeed = 5f;
    [SerializeField] private float _hatTiltingModifier = 5;
    [SerializeField] private Transform _characterSpriteTransform;
    [SerializeField] private Transform _hatSpriteTransform;

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

        var currentHatRotation = _hatSpriteTransform.rotation;
        var targetHatRotation = Quaternion.Euler(
                currentHatRotation.eulerAngles.x,
                currentHatRotation.eulerAngles.y,
                - tiltingAngle / _hatTiltingModifier
            );
        _hatSpriteTransform.rotation = Quaternion.Lerp(currentHatRotation, targetHatRotation, _tiltSpeed * _hatTiltingModifier * Time.deltaTime);
    }
}
