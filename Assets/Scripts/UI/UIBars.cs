using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBars : MonoBehaviour
{
    [SerializeField] Slider healthBar;
    [SerializeField] Slider shieldStaminaBar;
    [SerializeField] PlayerFighting playerFighting;

    void Awake() {
        playerFighting.changeHealth += ChangeHealth;
        playerFighting.changeShieldStamina += ChangeShieldStamina;
    }

    void ChangeHealth(float value) {
        healthBar.value = value;
    }

    void ChangeShieldStamina(float value) {
        shieldStaminaBar.value = value;
    }
}
