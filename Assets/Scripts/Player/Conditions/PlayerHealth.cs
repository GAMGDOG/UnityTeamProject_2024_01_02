using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private float _maxHealth = 100f;
    [SerializeField] private float _healthRegenerationRate = 0.5f;

    private float _health;
    public UnityAction OnDie;
    public UnityAction OnDamage;

    private UI_HUDPanel _uiHUD;

    public float Health
    {
        get { return _health; }
        private set { _health = value; }
    }

    public bool IsDead => _health == 0;

    private void Start()
    {
        ResetHealth();
        StartHealthRegeneration();
        OnDamage += SetDamageBackground;
        _uiHUD = FindObjectOfType<UI_HUDPanel>();
    }

    private void Update()
    {
        if (_health < _maxHealth)
        {
            StartHealthRegeneration();
        }
        //Debug.Log(_health);
    }

    private void ResetHealth()
    {
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
                }
            }
            yield return new WaitForSeconds(1f); // 1초마다 체크
        }
    }

    // 데미지 받는 부분도 OnDamage에 등록?
    private void TakeDamage(int damage)
    {
        if (_health <= 0) return;

        _health = Mathf.Max(_health - damage, 0);

        if (_health <= 0)
        {
            _health = 0.0f;
            CallDie();
        }
        CallDamage();
    }

    public void CallDamage()
    {
        OnDamage?.Invoke();
    }

    public void CallDie()
    {
        OnDie?.Invoke();
        Debug.Log("죽었어요.");
    }

    // 테스트용
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage(50);
            Debug.Log("적과 충돌! 현재 체력은 : " + _health);
        }
    }

    private void SetDamageBackground()
    {
        if (_uiHUD != null)
        {
            _uiHUD.StartDamageBackground();
        }
    }
}
