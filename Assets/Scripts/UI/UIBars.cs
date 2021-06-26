using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBars : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider shieldStaminaBar;
    [SerializeField] Slider rollCoolDown;
    [SerializeField] HealthComponent playerHealth;
    [SerializeField] PlayerFighting playerFighting;
    [SerializeField] PlayerController playerController;

    void Awake() {
        playerHealth.OnHealthChange += ChangeHealth;
        playerFighting.changeShieldStamina += ChangeShieldStamina;
        playerController.changeRollCoolDown += ChangeRollCoolDown;
    }

    void ChangeHealth(float current, float max) {
        healthBar.value = current / max;
    }

    void ChangeShieldStamina(float value) {
        shieldStaminaBar.value = value;
    }
    
    void ChangeRollCoolDown(float value) {
        rollCoolDown.value = value;
        if (rollCoolDown.value >= 1)
        {
            
            rollCoolDown.value = 0;
        }
    }
    
    
}
