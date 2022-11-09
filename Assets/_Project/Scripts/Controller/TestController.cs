using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ToolsBoxEngine;

public class TestController : MonoBehaviour {
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] EntityPhysicMovement _movement;
    [SerializeField] AimHelperReference _aimHelperReference;

    //bool _aiming = false;

    private void Reset() {
        _playerInput = GetComponent<PlayerInput>();
    }

    void Start() {
        _playerInput.actions["First Attack"].performed += Bump;
    }

    private void Update() {
        Vector2 direction = _playerInput.actions["Movements"].ReadValue<Vector2>();
        _movement.Move(direction);
    }

    private void Bump(InputAction.CallbackContext cc) {
        //_movement.Bump();
    }
}