using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighting : MonoBehaviour
{
    //attack
    [SerializeField] int comboHitsNumber = 3;
    [SerializeField] float maxComboDelay = 0.9f;
    int hitNumber = -1;
    float lastClickedTime = 0f;
    bool canAttack = true;
    public void SetCanAttack(bool value) {
        canAttack = value;
    }
    Animator _animatorLegs;
    Animator _animatorTop;
    void Start()
    {
        _animatorLegs = transform.GetChild(0).GetComponent<Animator>();
        _animatorTop = GetComponent<Animator>();
        _animatorTop.SetTrigger("attack");
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - lastClickedTime > maxComboDelay) {
            hitNumber = -1;
        }
        if (Input.GetButtonDown("Attack") && canAttack) {
            lastClickedTime = Time.time;
            hitNumber++;
            hitNumber = hitNumber % comboHitsNumber;
            Debug.Log("Attack " + hitNumber);
            _animatorTop.SetInteger("comboHitNumber", hitNumber);
            _animatorTop.SetTrigger("attack");
        }
        else {
            _animatorTop.ResetTrigger("attack");
        }
            
    }
}
