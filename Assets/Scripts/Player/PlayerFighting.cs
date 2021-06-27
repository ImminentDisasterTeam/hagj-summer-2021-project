using UnityEngine;
using System;

public class PlayerFighting : MonoBehaviour {
    [SerializeField] float damage = 15;

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
    [SerializeField] float maxComboDelay = 0.9f;
    int _hitNumber = -1;
    float _lastClickedTime;
    public bool canAttack = true;

    [SerializeField] private AttackAreaController[] attackAreas;
    private int comboHitsNumber;

    [SerializeField] private Collider2D playerCollider;

    //actions
    public Action<float> changeShieldStamina;

    Animator _animatorLegs;
    Animator _animatorTop;
    HealthComponent _healthComponent;

    void Awake()
    {
        foreach (var attackArea in attackAreas)
        {
            attackArea.damageTarget = DamageTarget;
        }
    }

    void Start() {
        _animatorLegs = transform.GetChild(0).GetComponent<Animator>();
        _animatorTop = GetComponent<Animator>();
        _currentShieldStamina = shieldStamina;
        _healthComponent = GetComponent<HealthComponent>();
        comboHitsNumber = attackAreas.Length;
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
        if (!other.IsTouching(playerCollider)) 
            return;
        if (!other.gameObject.CompareTag("Enemy") || !other.gameObject.CompareTag("EnemyHit") ) {
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
    
    public void DamageTarget(Collider2D enemy) {
        if (enemy.CompareTag("Enemy")) {
            enemy.GetComponent<HealthComponent>().TakeDamage(damage);
        }
    }
}
