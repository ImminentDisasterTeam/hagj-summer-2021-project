using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBossHP : MonoBehaviour
{
    [SerializeField] Slider healthBar;

    void Awake()
    {
        //set action to change boss hp here
    }

    void ChangeHealth(float current, float max)
    {
        healthBar.value = current / max;
    }
}
