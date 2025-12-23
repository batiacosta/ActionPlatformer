using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private SoundSO _gunShoot;

    private void OnEnable()
    {
        Gun.OnShot += GunOnShoot;
    }

    private void OnDisable()
    {
        Gun.OnShot -= GunOnShoot;
    }

    private void PlaySound(SoundSO soundSO)
    {
        var soundObject = new GameObject("Temp Audio Source");
        var audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = soundSO.Clip;
        audioSource.Play();
    }

    private void GunOnShoot()
    {
        PlaySound(_gunShoot);
    }
}
