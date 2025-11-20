using System;
using Miscelanius;
using UnityEngine;
using Random = UnityEngine.Random;


public class SpotlightBehaviour : MonoBehaviour, IPartyble
{
    [SerializeField] private GameObject _spotlightHead;
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _maxRotation = 45;
    [SerializeField] private float _partyRotationVelocity = 6;

    private float _currentRotation;
    private float _defaultRotationSpeed;

    private void Awake()
    {
        _defaultRotationSpeed = _rotationSpeed;
    }

    private void Start()
    {
        RandomStartingRotation();
    }

    private void OnEnable()
    {
        PartyManager.OnPartyStarts += StartParty;
        PartyManager.OnPartyStops += StopParty;
    }

    private void OnDisable()
    {
        PartyManager.OnPartyStarts -= StartParty;
        PartyManager.OnPartyStops -= StopParty;
    }

    
    public void StartParty()
    {
        _rotationSpeed = _defaultRotationSpeed * _partyRotationVelocity;
    }

    public void StopParty()
    {
        _rotationSpeed  = _defaultRotationSpeed;
    }
    private void Update()
    {
        RotateHead();
    }
    private void RandomStartingRotation()
    {
        var randomRotation = Random.Range(-_maxRotation, _maxRotation);
        _currentRotation = randomRotation;
    }

    private void RotateHead()
    {
        _currentRotation += Time.deltaTime * _rotationSpeed;
        var z = Mathf.PingPong(_currentRotation, _maxRotation);
        _spotlightHead.transform.localRotation = Quaternion.Euler(0, 0, z);
    }
}
