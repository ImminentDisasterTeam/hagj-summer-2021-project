using UnityEngine;
using System;

public class PlayerFighting : MonoBehaviour {
    //health
    // [SerializeField] float health = 100;
    [SerializeField] float damage = 15;
    // float _currentHealth;
    // void SetCurrentHealth(float value) {
    //     _currentHealth = value;
    //     changeHealth(_currentHealth/health);
    // }

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

    [SerializeField] LayerMask enemyLayers;

    [SerializeField] Vector2 combo1Offset;
    [SerializeField] float combo1Radius;

    //actions
    // public Action<float> changeHealth;
    public Action<float> changeShieldStamina;

    Animator _animatorLegs;
    Animator _animatorTop;
    HealthComponent _healthComponent;

    void Start() {
        _animatorLegs = transform.GetChild(0).GetComponent<Animator>();
        _animatorTop = GetComponent<Animator>();
        // _currentHealth = health;
        _currentShieldStamina = shieldStamina;
        _healthComponent = GetComponent<HealthComponent>();
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
        _healthComponent.TakeDamage(damage);
        // SetCurrentHealth(_currentHealth - damage);
    }

    void BlockHit(int damage) {
        if (_currentShieldStamina > 0) {
            Debug.Log("BLOCK");
            _animatorTop.SetTrigger("blockHit");
            _healthComponent.TakeDamage(damage * (100 - percentageOfBlockedDamage) / 100);
            SetCurrentShieldStamina(_currentShieldStamina - blockStaminaRequired);
        } else {
            _animatorTop.SetTrigger("shieldBroken");
            _healthComponent.TakeDamage(damage);
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

    public void ComboAttack1() {
        Vector2 attackCentre = transform.TransformPoint(combo1Offset);
        var enemies = Physics2D.OverlapCircleAll(attackCentre, combo1Radius, enemyLayers);
        foreach (var enemy in enemies) {
            enemy.GetComponent<HealthComponent>().TakeDamage(damage);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere
        Gizmos.color = Color.yellow;
        Vector2 attackCentre = transform.TransformPoint(combo1Offset);
        Gizmos.DrawSphere(attackCentre, combo1Radius);
    }
}
