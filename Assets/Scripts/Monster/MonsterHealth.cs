using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _healthRegenerationRate = 2f;

    private float _health;
    public event Action OnDie;
    public bool IsDead;

    public float Health
    {
        get { return _health; }
        private set { _health = value; }
    }
    private void Start()
    {
        ResetHealth();
    }

    private void Update()
    {
        if (IsDead)
            StartHealthRegeneration();
    }

    private void ResetHealth()
    {
        IsDead = false;
        _health = _maxHealth;
    }

    private void StartHealthRegeneration()
    {
        StartCoroutine(HealthRegeneration());
    }

    private IEnumerator HealthRegeneration()
    {
        while (_health < _maxHealth)
        {
            if (_health > 0)
            {
                _health += Time.deltaTime * _healthRegenerationRate;
                if (_health >= _maxHealth)
                {
                    _health = 100f;
                    IsDead = false;
                }
            }
            yield return new WaitForSeconds(1f); // 1초마다 체크
        }
    }
    public void TakeDamage(int damage)
    {
        if (_health <= 0) return;

        _health = Mathf.Max(_health - damage, 0);

        if (_health <= 0)
        {
            IsDead = true;
            OnDie?.Invoke();
            Debug.Log("괴물이 죽었어요.");
        }
    }
}
