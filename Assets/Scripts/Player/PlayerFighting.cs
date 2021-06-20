using UnityEngine;
using System;

public class PlayerFighting : MonoBehaviour {
    //health
    [SerializeField] float health = 100;
    float _currentHealth;
    void SetCurrentHealth(float value) {
        _currentHealth = value;
        changeHealth(_currentHealth/health);
    }

    //shield
    [SerializeField] float shieldAngleWidth = 90;
    [Range(0f, 100f)] float percentageOfBlockedDamage = 90;
    [SerializeField] float shieldStamina = 100;
    [SerializeField] float blockStaminaRequired = 10;
    float _currentShieldStamina;
    void SetCurrentShieldStamina(float value) {
        _currentShieldStamina = value;
        changeShieldStamina(_currentShieldStamina/shieldStamina);
    }

    //attack
    [SerializeField] int comboHitsNumber = 3;
    [SerializeField] float maxComboDelay = 0.9f;
    int _hitNumber = -1;
    float _lastClickedTime;
    public bool canAttack = true;

    //actions
    public Action<float> changeHealth;
    public Action<float> changeShieldStamina;

    Animator _animatorLegs;
    Animator _animatorTop;

    void Start() {
        _animatorLegs = transform.GetChild(0).GetComponent<Animator>();
        _animatorTop = GetComponent<Animator>();
        _currentHealth = health;
        _currentShieldStamina = shieldStamina;
    }

    void Update() {
        if (Time.time - _lastClickedTime > maxComboDelay) {
            _hitNumber = -1;
        }

        if (Input.GetButtonDown("Attack") && canAttack) {
            _lastClickedTime = Time.time;
            _hitNumber++;
            _hitNumber %= comboHitsNumber;
            Debug.Log("Attack " + _hitNumber);
            _animatorTop.SetInteger("comboHitNumber", _hitNumber);
            _animatorTop.SetTrigger("attack");
        } else {
            _animatorTop.ResetTrigger("attack");
        }

        _animatorTop.SetBool("holdShield", Input.GetButton("Block"));
    }

    void GetDamaged(int damage) {
        _animatorTop.SetTrigger("damaged");
        SetCurrentHealth(_currentHealth - damage);
    }

    void BlockHit(int damage) {
        if (_currentShieldStamina > 0) {
            Debug.Log("BLOCK");
            _animatorTop.SetTrigger("blockHit");
            SetCurrentHealth(_currentHealth - damage * (100 - percentageOfBlockedDamage) / 100);
            SetCurrentShieldStamina(_currentShieldStamina - blockStaminaRequired);
        } else {
            _animatorTop.SetTrigger("shieldBroken");
            SetCurrentHealth(_currentHealth - damage);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Enemy")) {
            return;
        }

        if (GetComponent<MoveSystem>().isHoldingShield) {
            var hitPoint = other.ClosestPoint(transform.position);
            Vector2 posPoint = transform.position;
            var hitDirection = hitPoint - posPoint;
           
            var angle = gameObject.GetComponent<PlayerController>().SignedAngleWithLookDirection(hitDirection);
            if (angle < shieldAngleWidth / 2) {
                BlockHit(10);
            } else {
                GetDamaged(10);
            }
        } else {
            GetDamaged(10);
        }
    }
}
