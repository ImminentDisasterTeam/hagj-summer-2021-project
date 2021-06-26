using System;
using UnityEngine;

public class BattleManager : MonoBehaviour{
    [SerializeField] HealthComponent playerHealth;
    [SerializeField] HealthComponent bossHealth;
    public Action OnWin;

    void Start() {
        playerHealth.OnDeath += () => {
            // suggest restart;
        };

        bossHealth.OnDeath += OnWin;
    }
}

