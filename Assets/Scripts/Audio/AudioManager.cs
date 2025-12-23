using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private float _masterVolume = 1;
    [SerializeField] private SoundSO _gunShoot;
    [SerializeField] private SoundSO _jumpSO;

    private void OnEnable()
    {
        Gun.OnShot += GunOnShoot;
        PlayerController.OnJump += OnJump;
    }

    private void OnDisable()
    {
        Gun.OnShot -= GunOnShoot;
        PlayerController.OnJump -= OnJump;
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
        SoundToPlay(_gunShoot);
    }
    private void OnJump()
    {
        SoundToPlay(_jumpSO);
    }
}
