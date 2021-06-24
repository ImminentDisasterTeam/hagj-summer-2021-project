using UnityEngine;

public class MoveSystem : MonoBehaviour {
    [SerializeField] float speed = 8;
    [SerializeField] float rollSpeed = 16;
    [SerializeField] float speedWithShield = 3;
    public bool isHoldingShield;

    Rigidbody2D _rb;
    public Vector2 _lastLookDirection;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _lastLookDirection = _rb.transform.forward;
    }
    public void Move(Vector2 moveDirection) {
        _rb.velocity = moveDirection.normalized * (isHoldingShield ? speedWithShield : speed);
    }

    public void Roll(Vector2 rollDirection) {
        if (rollDirection == Vector2.zero) {
            rollDirection = _lastLookDirection;
        }
        
        _rb.velocity = rollDirection.normalized * rollSpeed;
    }

    public void Turn(Vector2 lookDirection) {
        if (lookDirection == Vector2.zero) {
            var velocity = _rb.velocity;
            lookDirection = velocity == Vector2.zero ? _lastLookDirection : velocity;
        }
        _lastLookDirection = lookDirection;

        var angle = Vector2.SignedAngle(transform.up, lookDirection);
        transform.Rotate(Vector3.forward, angle);
    }
}
