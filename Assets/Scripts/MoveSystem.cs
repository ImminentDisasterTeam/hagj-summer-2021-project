using UnityEngine;

public class MoveSystem : MonoBehaviour {
    // TODO: rework (remove acceleration & deceleration, rotation speed and MAYBE merge move and turn
    public float acceleration = 10;
    public float deceleration = 15;
    public float speed = 8;
    public float rotationSpeed = 1440;

    Rigidbody2D _rb;
    Vector2 _lastLookDirection;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _lastLookDirection = _rb.transform.forward;
    }
    public void Move(Vector2 moveDirection) {
        _rb.MovePosition(_rb.position + moveDirection.normalized * (speed * Time.fixedDeltaTime));
    }

    Vector2 ApplySpeedConstraint(Vector2 speed, float maxSpeed) {
        if (speed.magnitude <= maxSpeed) {
            return speed;
        }

        return speed.normalized * maxSpeed;
    }

    public void Turn(Vector2 lookDirection) {
        if (lookDirection == Vector2.zero) {
            var velocity = _rb.velocity;
            lookDirection = velocity == Vector2.zero ? _lastLookDirection : velocity;
        }
        _lastLookDirection = lookDirection;

        var rawAngle = Vector2.SignedAngle(transform.up, lookDirection);
        var turnDirection = Mathf.Sign(rawAngle);
        var maxRotation = rotationSpeed * Time.deltaTime;

        var angle = turnDirection * Mathf.Min(Mathf.Abs(rawAngle), maxRotation);
        transform.Rotate(Vector3.forward, angle);
    }
}
