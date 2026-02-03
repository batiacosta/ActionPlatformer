using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    [Range(0f, 2f)]
    [SerializeField] private float _masterVolume = 1;
    [SerializeField] private SoundsCollectionSO _soundsCollectionSO;
    [SerializeField] private AudioMixerGroup _sfxMixerGroup;
    [SerializeField] private AudioMixerGroup _musicMixerGroup;

    private AudioSource _currentAudioSource;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        FightMusic();
    }

    private void OnEnable()
    {
        Gun.OnShot += GunOnShoot;
        Gun.OnGrenateShoot += GunOnGrenadeShoot;
        PlayerController.OnJump += OnJump;
        PlayerController.OnJetpack += OnJetpack;
        Health.OnDeath += HandleDeath;
        PartyManager.OnDiscoBallHit += DiscoBallMusic;
    }

    private void OnDisable()
    {
        Gun.OnShot -= GunOnShoot;
        Gun.OnGrenateShoot -= GunOnGrenadeShoot;
        PlayerController.OnJump -= OnJump;
        PlayerController.OnJetpack -= OnJetpack;
        Health.OnDeath -= HandleDeath;
        PartyManager.OnDiscoBallHit -= DiscoBallMusic;
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
        AudioMixerGroup audioMixerGroup;
        pitch = RandomizePitch(soundSO, pitch);

        audioMixerGroup = DetermineAudioMixerGroup(soundSO);
        PlaySound(clip, pitch, volume, loop, audioMixerGroup);
    }
    private float RandomizePitch(SoundSO soundSO, float pitch)
    {
        if (soundSO.RandomizePitch)
        {
            var randomPitchModifier = Random.Range(-soundSO.RandomPitchRangeModifier, soundSO.RandomPitchRangeModifier);
            pitch += randomPitchModifier;
        }

        return pitch;
    }

    private AudioMixerGroup DetermineAudioMixerGroup(SoundSO soundSO)
    {
        AudioMixerGroup audioMixerGroup;
        switch (soundSO.AudioType)
        {
            case SoundSO.AudioTypes.Music:
                audioMixerGroup = _musicMixerGroup;
                break;
            case SoundSO.AudioTypes.SFX:
                audioMixerGroup = _sfxMixerGroup;
                break;
            default:
                audioMixerGroup = null;
                break;
        }

        return audioMixerGroup;
    }
    
    private void PlaySound(AudioClip clip, float pitch, float volume, bool loop, AudioMixerGroup audioMixerGroup)
    {
        var soundObject = new GameObject("Temp Audio Source");
        var audioSource = soundObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.pitch = pitch;
        audioSource.outputAudioMixerGroup = audioMixerGroup;
        audioSource.Play();

        if (!loop) Destroy(soundObject, clip.length);
        DetermineMusicMixerGroup(audioMixerGroup, audioSource);
}

    private void DetermineMusicMixerGroup(AudioMixerGroup audioMixerGroup, AudioSource audioSource)
    {
        if (audioMixerGroup == _musicMixerGroup)
        {
            if(_currentAudioSource != null) _currentAudioSource.Stop();
            
            _currentAudioSource = audioSource;
        }
    }

    #region VFX
    
    public void GrenadeOnBeep()
    {
        PlayRandomSound(_soundsCollectionSO.GrenadeBeep);
    }

    public void OnGrenadeExplode()
    {
        PlayRandomSound(_soundsCollectionSO.GrenadeExplosions);
    }
    
    public void EnemyOnPlayerHit()
    {
        PlayRandomSound(_soundsCollectionSO.PlayerHit);
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
    
    private void OnDeath()
    {
        PlayRandomSound(_soundsCollectionSO.Splat);
    }

    private void OnJetpack()
    {
        PlayRandomSound(_soundsCollectionSO.Jetpack);
    }

    private void AudioManagerMegaKill()
    {
        PlayRandomSound(_soundsCollectionSO.MegaKill);
    }

    

    private void GunOnGrenadeShoot()
    {
        PlayRandomSound(_soundsCollectionSO.GrenadeShoot);
    }
    
    
    
    #endregion
    
    #region Music
    private void FightMusic()
    {
        PlayRandomSound(_soundsCollectionSO.FightMusic);
    }

    private void DiscoBallMusic()
    {
        PlayRandomSound(_soundsCollectionSO.DiscoBallMusic);
        var soundLength = _soundsCollectionSO.DiscoBallMusic[0].Clip.length;
        Miscelanius.Utils.RunAfterDelay(this, soundLength, FightMusic);
    }
    #endregion
    
    #region Custom SFX Logic
    private List<Health> _deathList = new List<Health>();
    private Coroutine _deathCoroutine;

    private void HandleDeath(Health health)
    {
        if (health.gameObject.TryGetComponent(out Enemy enemy)) _deathList.Add(health);

        if(_deathCoroutine == null) StartCoroutine(DeathWindowRoutine());
    }

    private IEnumerator DeathWindowRoutine()
    {
        Debug.Log($"Inicia coroutina {_deathList.Count}");
        yield return null;
        int megakillAmount = 3;
        if (_deathList.Count >= megakillAmount)
        {
            Debug.Log("Megakill amount is " + megakillAmount);
            AudioManagerMegaKill();
        }
        
        OnDeath();
        _deathList.Clear();
        _deathCoroutine = null;
    }
    #endregion
}
