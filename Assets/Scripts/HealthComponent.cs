using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour {
    [SerializeField] float maxHealth;
    float _currentHealth;
    public Action<float, float> OnHealthChange;
    public Action OnDeath;

    void Start() {
        _currentHealth = maxHealth;
    }

    public void TakeDamage(float damage) {
        _currentHealth = Mathf.Max(0, _currentHealth - damage);
        OnHealthChange?.Invoke(_currentHealth, maxHealth);
        
        if (_currentHealth == 0) {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}
