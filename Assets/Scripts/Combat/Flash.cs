using System;
using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _whiteFlashMaterial;
    [SerializeField] private float _flashTime = 0.1f;

    private SpriteRenderer[] _spriteRenderers;
    private ColorChanger _colorChanger;
    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        _colorChanger = GetComponent<ColorChanger>();
    }

    public void StartFlash()
    {
        StartCoroutine(FlashingRoutine());
    }

    private IEnumerator FlashingRoutine()
    {
        foreach (var spriteRenderer in _spriteRenderers)
        {
            spriteRenderer.material = _whiteFlashMaterial;
            _colorChanger?.SetColor(Color.white);
        }
        
        yield return new WaitForSeconds(_flashTime);
        SetDefaultMaterial();
    }

    private void SetDefaultMaterial()
    {
        foreach (var spriteRenderer in _spriteRenderers) spriteRenderer.material = _defaultMaterial;
        _colorChanger?.SetColor(_colorChanger.DefaultColor);
    }
}
