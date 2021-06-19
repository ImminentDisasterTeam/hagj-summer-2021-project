using UnityEngine;

public class PlayerController : MonoBehaviour {
    InputDetector _inputDetector;
    Vector2 _moveDirection;
    Vector2 _lookDirection;

    MoveSystem _moveSystem;
    Camera _mainCamera;

    const string ControllerPrefix = "Controller";
    
    void Start() {
        _inputDetector = GetComponent<InputDetector>();
        _moveSystem = GetComponent<MoveSystem>();
        _mainCamera = Camera.main;
    }

    void Update() {
        GetInputs(_inputDetector.State);
    }

    void GetInputs(InputDetector.EInputState inputState) {
        var prefix = "";
        if (inputState == InputDetector.EInputState.Controller) {
            prefix = ControllerPrefix;
        }
        
        var moveHorizontal = Input.GetAxisRaw(prefix + "MoveHorizontal");
        var moveVertical = Input.GetAxisRaw(prefix + "MoveVertical");
        _moveDirection = new Vector2(moveHorizontal, moveVertical);

        if (inputState == InputDetector.EInputState.MouseKeyboard) {
            var mousePos = _mainCamera.ScreenToWorldPoint(Input.mousePosition);
            _lookDirection = (mousePos - transform.position);
        } else {
            var lookHorizontal = Input.GetAxisRaw("ControllerLookHorizontal");
            var lookVertical = Input.GetAxisRaw("ControllerLookVertical");
            _lookDirection = new Vector2(lookHorizontal, lookVertical);
        }
    }

    void FixedUpdate() {
        _moveSystem.Move(_moveDirection);
        _moveSystem.Turn(_lookDirection);
    }
}
