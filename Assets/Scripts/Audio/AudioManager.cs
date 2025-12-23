using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float _masterVolume = 1;
    [SerializeField] private SoundsCollectionSO _soundsCollectionSO;
    private void OnEnable()
    {
        Gun.OnShot += GunOnShoot;
        PlayerController.OnJump += OnJump;
        Health.OnDeath += OnDeath;
    }

    private void OnDisable()
    {
        Gun.OnShot -= GunOnShoot;
        PlayerController.OnJump -= OnJump;
        Health.OnDeath -= OnDeath;
    }

    private void PlayRandomSound(SoundSO[] soundsSO)
    {
        if (soundsSO != null && soundsSO.Length > 0)
        {
            var soundSO = soundsSO[Random.Range(0, soundsSO.Length)];
            SoundToPlay(soundSO);
        }
    }
    private void SoundToPlay(SoundSO soundSO)
    {
        var clip = soundSO.Clip;
        var pitch = soundSO.Pitch;
        var volume = soundSO.Volume * _masterVolume;
        var loop =  soundSO.Loop;
        if (soundSO.RandomizePitch)
        {
            var randomPitchModifier = Random.Range(-soundSO.RandomPitchRangeModifier, soundSO.RandomPitchRangeModifier);
            pitch += randomPitchModifier;
        }
        
        PlaySound(clip, pitch, volume, loop);
    }
    private void PlaySound(AudioClip clip,  float pitch, float volume, bool loop)
    {
        var soundObject = new GameObject("Temp Audio Source");
        var audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.pitch = pitch;
        audioSource.Play();
        
        if(!loop) Destroy(soundObject, clip.length);
    }

    private void GunOnShoot()
    {
        PlayRandomSound(_soundsCollectionSO.GunShoot);
    }
    private void OnJump()
    {
        PlayRandomSound(_soundsCollectionSO.Jump);
    }

    private void OnDeath(Health health)
    {
        PlayRandomSound(_soundsCollectionSO.Splat);
    }
}
