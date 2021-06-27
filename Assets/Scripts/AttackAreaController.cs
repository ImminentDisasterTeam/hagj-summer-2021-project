using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackAreaController : MonoBehaviour
{
    public Action<Collider2D> damageTarget;

    void OnTriggerEnter2D(Collider2D collider)
    {
        damageTarget?.Invoke(collider);
    }
}
