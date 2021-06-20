using UnityEngine;

public class PlayerController : MonoBehaviour {
    InputDetector _inputDetector;
    Vector2 _moveDirection;
    Vector2 _lookDirection;

    MoveSystem _moveSystem;
    Camera _mainCamera;
    Animator _animatorLegs;
    Animator _animatorTop;
    [SerializeField] float angleOfVerticalMovement = 90f;

    bool isControllable = true;

    public void SetIsControllable(bool value) {
        isControllable = value;
    }

    const string ControllerPrefix = "Controller";
    
    void Start() {
        _inputDetector = GetComponent<InputDetector>();
        _moveSystem = GetComponent<MoveSystem>();
        _mainCamera = Camera.main;
        _animatorLegs = transform.GetChild(0).GetComponent<Animator>();
        _animatorTop = GetComponent<Animator>();
        Debug.Log(_animatorLegs.gameObject.name);
    }

    void Update() {
        GetInputs(_inputDetector.State);
    }

    void GetInputs(InputDetector.EInputState inputState) {
        var prefix = "";
        if (inputState == InputDetector.EInputState.Controller) {
            prefix = ControllerPrefix;
        }
        
        //movement
        var moveHorizontal = Input.GetAxisRaw(prefix + "MoveHorizontal");
        var moveVertical = Input.GetAxisRaw(prefix + "MoveVertical");
        _moveDirection = new Vector2(moveHorizontal, moveVertical);
        if (_moveDirection.Equals(Vector3.zero)) {
            Debug.Log("Idle");
            _animatorLegs.SetBool("isMoving", false);
            _animatorTop.SetBool("isMoving", false);
        }
        else {
            Debug.Log("Moving");
            _animatorLegs.SetBool("isMoving", true);
            _animatorTop.SetBool("isMoving", true);
        }
        
        //rotation
        if (inputState == InputDetector.EInputState.MouseKeyboard) {
            var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _lookDirection = (mousePos - transform.position);
        } else {
            var lookHorizontal = Input.GetAxisRaw("ControllerLookHorizontal");
            var lookVertical = Input.GetAxisRaw("ControllerLookVertical");
            _lookDirection = new Vector2(lookHorizontal, lookVertical);
        }

        //angle of movement
        float angle = signedAngleBetween(_moveDirection, _lookDirection);
        if (angle < angleOfVerticalMovement/2 || angle > 90 + angleOfVerticalMovement/2) {
            Debug.Log("Vertical Movement");
            _animatorLegs.SetBool("moveVertical", true);
        }
        else {
            Debug.Log("Horizontal Movement");
            _animatorLegs.SetBool("moveVertical", false);
        }
    }

    void FixedUpdate() {
        _moveSystem.Move(_moveDirection);
        _moveSystem.Turn(_lookDirection);
    }

    float signedAngleBetween (Vector3 a, Vector3 b) {
        float angle = Vector3.Angle(a, b);

        if( Mathf.Sign(angle) == -1)
            angle = 360 + angle;

        return angle;
    }
}
