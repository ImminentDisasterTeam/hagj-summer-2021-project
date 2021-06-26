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

    [SerializeField] LayerMask enemyLayers;

    [SerializeField] Vector2 combo1Offset;
    [SerializeField] float combo1Radius;
    [SerializeField] Vector2 combo2Offset;
    [SerializeField] float combo2Radius;
    [SerializeField] Vector2 combo3Offset;
    [SerializeField] float combo3Radius;

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
    
    public void ComboAttack1() {
        Vector2 attackCentre = transform.TransformPoint(combo1Offset);
        var enemies = Physics2D.OverlapCircleAll(attackCentre, combo1Radius, enemyLayers);
        foreach (var enemy in enemies) {
            enemy.GetComponent<EnemyController>().TakeDamage();
        }
    }

    public void ComboAttack2() {
        Vector2 attackCentre = transform.TransformPoint(combo2Offset);
        var enemies = Physics2D.OverlapCircleAll(attackCentre, combo2Radius, enemyLayers);
        foreach (var enemy in enemies) {
            enemy.GetComponent<EnemyController>().TakeDamage();
        }
    }
    
    public void ComboAttack3()
    {
        Vector2 attackCentre = transform.TransformPoint(combo3Offset);
        Vector2 leftBottomPoint = attackCentre;
        Vector2 rightTopPoint = attackCentre + new Vector2(combo3Radius, 2*combo3Radius);
        var enemies = Physics2D.OverlapAreaAll(leftBottomPoint, rightTopPoint, enemyLayers);
        foreach (var enemy in enemies) {
            enemy.GetComponent<EnemyController>().TakeDamage();
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere
        // Gizmos.color = Color.yellow;
        // Vector2 attackCentre = transform.TransformPoint(combo1Offset);
        // Gizmos.DrawSphere(attackCentre, combo1Radius);
        
        // Gizmos.color = Color.green;
        // Vector2 attackCentre = transform.TransformPoint(combo2Offset);
        // Gizmos.DrawSphere(attackCentre, combo2Radius);
        
        Gizmos.color = Color.red;
        Vector3 attackCentre = transform.TransformPoint(combo3Offset);
        // Gizmos.DrawCube(attackCentre, combo3Radius);
        Gizmos.DrawCube(attackCentre, new Vector3(combo3Radius, 2*combo3Radius, 0f));
    }
}
