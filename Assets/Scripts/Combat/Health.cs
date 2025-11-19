 using System;
 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public Action OnDeath;
    [SerializeField] private GameObject _splatterPrefab;
    [SerializeField] private GameObject _deathParticlesPrefab ;
    [SerializeField] private int _startingHealth = 3;

    private int _currentHealth;

    private void Start() {
        ResetHealth();
    }

    private void OnEnable()
    {
        OnDeath += SpawnDeathVFX;
        OnDeath += SpawnDeathSplatterPrefab;
    }

    private void OnDisable()
    {
        OnDeath -= SpawnDeathVFX;
        OnDeath -= SpawnDeathSplatterPrefab;
    }

    public void ResetHealth() {
        _currentHealth = _startingHealth;
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;

        if (_currentHealth <= 0) {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }

    private void SpawnDeathSplatterPrefab()
    {
        var splatterInstance = Instantiate(_splatterPrefab, transform.position, Quaternion.identity);
        var colorChanger = gameObject.GetComponent<ColorChanger>();
        splatterInstance.GetComponent<SpriteRenderer>().color = colorChanger.DefaultColor;
    }

    private void SpawnDeathVFX()
    {
        var particlesInstance = Instantiate(_deathParticlesPrefab, transform.position, Quaternion.identity);
        var particles = particlesInstance.GetComponent<ParticleSystem>().main;
        var colorChanger = gameObject.GetComponent<ColorChanger>();
        particles.startColor = colorChanger.DefaultColor;
    }
}
