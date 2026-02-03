using System;
using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _respawnPoint;
    [SerializeField] private float _fadeTime = 1.5f;

    private Image _image;
    private CinemachineCamera _cinemachineCamera;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _cinemachineCamera = FindFirstObjectByType<CinemachineCamera>();
    }

    public void FadeIn()
    {
        Debug.Log("FadeIn");
        StartCoroutine(FadeRoutine(1f));
    }

    private void FadeOut()
    {
        StartCoroutine(FadeRoutine(0f));
    }

    private IEnumerator FadeRoutine(float targetAlpha)
    {
        var elapsedTime = 0f;
        var startValue = _image.color.a;
        while (elapsedTime < _fadeTime)
        {
            elapsedTime += Time.deltaTime;
            var newAlpha = Mathf.Lerp(startValue, targetAlpha, elapsedTime / _fadeTime);
            _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, newAlpha);
            yield return null;
        }
        _image.color = new Color(_image.color.r, _image.color.g, _image.color.b, targetAlpha);
    }
}
