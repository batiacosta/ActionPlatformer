using System;
using UnityEngine;
using Random = UnityEngine.Random;


public class SpotlightBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _spotlightHead;
    [SerializeField] private float _rotationSpeed = 20f;
    [SerializeField] private float _maxRotation = 45;

    private float _currentRotation;

    private void Start()
    {
        RandomStartingRotation();
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
