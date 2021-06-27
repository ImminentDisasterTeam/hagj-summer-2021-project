using System;
using System.Collections;
using UnityEngine;

public class EnkiduController : MonoBehaviour{
    [SerializeField] Transform player;
    Rigidbody2D _rb;
    Animator _animator;
    HealthComponent _healthComponent;
    HealthComponent _playerHealth;

    [SerializeField] float moveSpeed = 1;
    [SerializeField] float chargeSpeed = 3;
    [SerializeField] float chargeDamage = 30;
    [SerializeField] float recoverAfterDashTime = 1;
    [SerializeField] float chargeDistance = 4;
    [SerializeField] float powerupHealthPart = 0.3f;
    const float StrikeDistance = 1.4f;
    const float MoveTime = 0.03f;

    Coroutine _currentAction;
    public bool poweruped;
    
    public bool rollPrepared;
    public bool finishedBeingDamaged;
    public bool attackStopped;
    
    static readonly int Roll = Animator.StringToHash("roll");
    static readonly int Upgrade = Animator.StringToHash("upgrade");
    static readonly int Damaged = Animator.StringToHash("damaged");
    static readonly int Moving = Animator.StringToHash("isMoving");
    static readonly int AttackHands = Animator.StringToHash("attackHands");

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        
        _healthComponent = GetComponent<HealthComponent>();
        _healthComponent.OnHealthChange = (current, max) => {
            if (!poweruped && current / max <= powerupHealthPart) {
                StopCoroutine(_currentAction);
                
                // TODO: SET ALL BOOLS FALSE
                _rb.velocity = Vector2.zero;
                rollPrepared = false;
                finishedBeingDamaged = false;
                attackStopped = false;
                
                moveSpeed *= 1.5f;
                chargeSpeed *= 1.5f;
                recoverAfterDashTime /= 1.5f;
                // attackTime /= 1.5f;
            
                _currentAction = StartCoroutine(Powerup());
                return;
            }

            StartCoroutine(AfterHitInvulnerability());
        };
        
        _playerHealth = player.GetComponent<HealthComponent>();
        
        SetCurrentAction();
    }

    IEnumerator GetCurrentAction() {
        var distanceToPlayerVector = player.position - transform.position;
        var distanceToPlayerDirection = distanceToPlayerVector.normalized;
        var distanceToPlayer = distanceToPlayerVector.magnitude;
        
        if (distanceToPlayer >= chargeDistance) {
            return Charge(distanceToPlayerVector);
        }
        
        if (distanceToPlayer <= StrikeDistance) {
            return Strike(distanceToPlayerDirection);
        }
        
        return Move(distanceToPlayerDirection);
    }

    void SetCurrentAction() {
        _currentAction = StartCoroutine(GetCurrentAction());
    }

    IEnumerator Charge(Vector2 chargeVector) {
        var direction = chargeVector.normalized;
        transform.up = direction;
        
        _animator.SetBool(Roll, true);
        yield return new WaitUntil(() => rollPrepared);
        
        rollPrepared = false;
        _rb.velocity = direction * chargeSpeed;
        yield return new WaitForSeconds(chargeVector.magnitude / chargeSpeed);

        _animator.SetBool(Roll, false);
        _rb.velocity = Vector2.zero;
        yield return new WaitForSeconds(recoverAfterDashTime);
        
        SetCurrentAction();
    }
    
    IEnumerator Strike(Vector2 direction) {
        transform.up = direction;
        
        _animator.SetTrigger(AttackHands);
        yield return new WaitUntil(() => attackStopped);

        attackStopped = false;
        
        SetCurrentAction();
    }
    
    IEnumerator Move(Vector2 direction) {
        transform.up = direction;
        _animator.SetBool(Moving, true);
        
        _rb.velocity = direction * moveSpeed;
        yield return new WaitForSeconds(MoveTime);
       
        _animator.SetBool(Moving, false);
        _rb.velocity = Vector2.zero;
        
        SetCurrentAction();
    }
    
    IEnumerator Powerup() {
        _animator.SetTrigger(Upgrade);
        
        _healthComponent.invulnerable = true;
        yield return new WaitUntil(() => poweruped);
        
        _healthComponent.invulnerable = false;
        
        SetCurrentAction();
    }

    IEnumerator AfterHitInvulnerability() {
        _animator.SetTrigger(Damaged);
        _healthComponent.invulnerable = true;
        yield return new WaitUntil(() => finishedBeingDamaged);
        
        finishedBeingDamaged = false;
        _healthComponent.invulnerable = false;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player") && _animator.GetBool(Roll)) {
            // _playerHealth.TakeDamage(chargeDamage);
        }
    }
}
