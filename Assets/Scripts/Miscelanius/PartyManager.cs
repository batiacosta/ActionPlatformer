using System;
using System.Collections;
using Miscelanius;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PartyManager : MonoBehaviour
{
    public static Action OnPartyStarts;
    public static Action OnPartyStops;
    public static Action OnDiscoBallHit;

    [SerializeField] private float _partyTime = 2;
    [SerializeField] private Light2D _globalLight;
    [SerializeField] private float _globalLightPartyIntensity;
    
    private bool _isPartying = false;
    private float _defaultGlobalLightIntensity;

    private void Awake()
    {
        _defaultGlobalLightIntensity = _globalLight.intensity;
    }

    private void OnEnable()
    {
        OnDiscoBallHit += StartParty;
        OnPartyStarts += PartyStarted;
        OnPartyStops += PartyStopped;
    }

    

    private void OnDisable()
    {
        OnDiscoBallHit -= StartParty;
        OnPartyStarts -= PartyStarted;
        OnPartyStops -= PartyStopped;
    }

    private void StartParty()
    {
        if (_isPartying) return;
        StartCoroutine(PartyRoutine());
    }

    private IEnumerator PartyRoutine()
    {
        _isPartying = true;
        OnPartyStarts?.Invoke();
        yield return new WaitForSeconds(_partyTime);
        _isPartying = false;
        OnPartyStops?.Invoke();
    }

    private void PartyStarted()
    {
        _globalLight.intensity = _globalLightPartyIntensity;
    }

    private void PartyStopped()
    {
        _globalLight.intensity = _defaultGlobalLightIntensity;
    }
}
