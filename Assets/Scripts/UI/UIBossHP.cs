using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHP : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] HealthComponent bothHealth;

    void Start() {
        bothHealth.OnHealthChange += ChangeHealth;
    }

    void ChangeHealth(float current, float max)
    {
        healthBar.value = current / max;
    }
}
