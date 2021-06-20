using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFighting : MonoBehaviour
{
    //health
    [SerializeField] float health = 100;
    float curHealth;
    //shield
    [SerializeField] float shieldRadius = 90;
    [Range(0f, 100f)] float percentageOfBlockedDamage = 90;
    [SerializeField] float shieldStamina = 100;
    [SerializeField] float BlockStaminaRequire = 10;
    float curShieldStamina;
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
        curHealth = health;
        curShieldStamina = shieldStamina;
    }

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

        if (Input.GetButton("Block")) {
            _animatorTop.SetBool("holdShield", true);
        }
        else {
            _animatorTop.SetBool("holdShield", false);
        }
    }

    void BeDamaged(int damage) {
        _animatorTop.SetTrigger("damaged");
        curHealth -= damage;
    }

    void BlockHit(int damage) {
        if (shieldStamina > 0) {
            Debug.Log("BLOCK");
            _animatorTop.SetTrigger("blockHit");
            curHealth -= damage*(100-percentageOfBlockedDamage)/100;
            shieldStamina -= BlockStaminaRequire;
        } else {
            _animatorTop.SetTrigger("shieldBroken");
            curHealth -= damage;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            if (GetComponent<MoveSystem>().isHoldingShield) {
                Vector2 hitPoint = other.ClosestPoint(transform.position);
                Vector2 posPoint = transform.position;
                Vector2 hitDirection = hitPoint - posPoint;
                float angle = gameObject.GetComponent<PlayerController>().SignedAngleWithLookDirection(hitDirection);
                if (angle < shieldRadius/2) {
                    BlockHit(10);
                } else {
                    BeDamaged(10);
                }
            } else {
                BeDamaged(10);
            }
        }
    }
}
