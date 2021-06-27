using UnityEngine;
using System.Collections;
using System;

public class PlayerController : MonoBehaviour {
    InputDetector _inputDetector;
    Vector2 _moveDirection;
    Vector2 _lookDirection;
    MoveSystem _moveSystem;
    Camera _mainCamera;
    Animator _animatorLegs;
    Animator _animatorTop;
    [SerializeField] float angleOfVerticalMovement = 90f;
    [SerializeField] float rollCoolDown = 3f;
    bool _rollReady = true;
    
    public bool CanMove { set; private get; }
    public bool IsRolling { set; private get; }
    public bool CanRoll { set; private get; } = true;

    const string ControllerPrefix = "Controller";
    
    public Action<float> changeRollCoolDown;
    
    void Start() {
        _inputDetector = GetComponent<InputDetector>();
        _moveSystem = GetComponent<MoveSystem>();
        _mainCamera = Camera.main;
        _animatorLegs = transform.GetChild(0).GetComponent<Animator>();
        _animatorTop = GetComponent<Animator>();
    }

    void Update() {
        GetInputs(_inputDetector.State);
        RollDetector();
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
        if (!CanMove) {
            _moveDirection = Vector2.zero;
        }
        if (_moveDirection.Equals(Vector2.zero)) {
            _animatorLegs.SetBool("isMoving", false);
            _animatorTop.SetBool("isMoving", false);
        }
        else {
            _animatorLegs.SetBool("isMoving", true);
            _animatorTop.SetBool("isMoving", true);
        }
        
        //rotation
        if (!IsRolling) {
            if (inputState == InputDetector.EInputState.MouseKeyboard) {
                var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
                _lookDirection = (mousePos - transform.position);
            } else {
                var lookHorizontal = Input.GetAxisRaw("ControllerLookHorizontal");
                var lookVertical = Input.GetAxisRaw("ControllerLookVertical");
                _lookDirection = new Vector2(lookHorizontal, lookVertical);
            }
        }

        //angle of movement
        var angle = SignedAngleWithLookDirection(_moveDirection);
        if (angle < angleOfVerticalMovement/2 || angle > 90 + angleOfVerticalMovement/2) {
            _animatorLegs.SetBool("moveVertical", true);
        } else {
            _animatorLegs.SetBool("moveVertical", false);
        }
    }

    void RollDetector() {
        if (_rollReady == false || !CanRoll) 
            return;
        if (Input.GetButtonDown("Dodge")) {
            StartCoroutine(RollCoolDownCoro(rollCoolDown));
            _animatorTop.SetTrigger("roll");
        } else {
            _animatorTop.ResetTrigger("roll");
        }
    }

    IEnumerator RollCoolDownCoro(float rollCoolDown) {
        _rollReady = false;
        float delta = 0.01f;
        for (float i = 0; i < rollCoolDown; i += delta)
        {
            changeRollCoolDown(i/rollCoolDown);
            yield return new WaitForSeconds(delta);
        }
        changeRollCoolDown(1);
        _rollReady = true;
    }


    void FixedUpdate() {
        _moveSystem.Move(_moveDirection);
        if (!IsRolling) {
            _moveSystem.Turn(_lookDirection);
        }
        if (IsRolling) {
            _moveSystem.Roll(_moveDirection);
        }
    }

    public float SignedAngleWithLookDirection(Vector2 a) {
        var angle = Vector2.Angle(a, _lookDirection);

        if(Mathf.Sign(angle) == -1) {
            angle = 360 + angle;
        }

        return angle;
    }
}
