using System;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    
    [SerializeField] private TMP_Text _scoreText;

    private int _currentScore = 0;

    private void Start()
    {
        _currentScore = 0;
    }

    private void OnEnable()
    {
        Health.OnDeath += EnemyOnDeath;
    }

    private void OnDisable()
    {
        Health.OnDeath -= EnemyOnDeath;
    }

    private void EnemyOnDeath(Health health)
    {
        if (!health.gameObject.TryGetComponent(out Enemy enemy)) return;
        _currentScore++;
        _scoreText.text = _currentScore.ToString("D3");
    }
}
