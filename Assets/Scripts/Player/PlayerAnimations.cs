 using System;
 using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [SerializeField] private ParticleSystem _moveDustParticles;

    private void Update()
    {
        DetectMoveDust();
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
}
