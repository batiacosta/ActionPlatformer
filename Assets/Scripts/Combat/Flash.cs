using System;
using System.Collections;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material _defaultMaterial;
    [SerializeField] private Material _whiteFlashMaterial;
    [SerializeField] private float _flashTime = 0.1f;

    private SpriteRenderer[] _spriteRenderers;
    private Color _flashColor = Color.white;

    private void Awake()
    {
        _spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
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
            spriteRenderer.color = _flashColor;
        }
        
        yield return new WaitForSeconds(_flashTime);
        SetDefaultMaterial();
    }

    private void SetDefaultMaterial()
    {
        foreach (var spriteRenderer in _spriteRenderers) spriteRenderer.material = _defaultMaterial;
    }
}
