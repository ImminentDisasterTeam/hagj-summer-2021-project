using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBars : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider shieldStaminaBar;
    [SerializeField] Slider rollCoolDown;
    [SerializeField] PlayerFighting playerFighting;
    [SerializeField] PlayerController playerController;

    void Awake() {
        playerFighting.changeHealth += ChangeHealth;
        playerFighting.changeShieldStamina += ChangeShieldStamina;
        
        playerController = playerFighting.gameObject.GetComponent<PlayerController>();
        playerController.changeRollCoolDown += ChangeRollCoolDown;
    }

    void ChangeHealth(float value) {
        healthBar.value = value;
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
