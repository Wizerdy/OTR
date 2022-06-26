using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] PlayerEntity _player;

    bool _setupThrow = false;

    private void Reset() {
        _playerInput = GetComponent<PlayerInput>();
    }

    void Start() {
        _playerInput.actions["Aim"].performed += _Aim;
        _playerInput.actions["Attack"].performed += _Attack;
        _playerInput.actions["Throw"].performed += _Throw;

        _playerInput.actions["SetupThrow"].performed += _SetupThrow;
        _playerInput.actions["SetupThrow"].canceled += _SetupThrow;
    }

    private void Update() {
        _player.Move(_playerInput.actions["Movements"].ReadValue<Vector2>());
    }

    private void _Aim(InputAction.CallbackContext cc) {
        _player.LookAt(cc.ReadValue<Vector2>());
    }

    private void _Attack(InputAction.CallbackContext cc) {
        if (_setupThrow) { return; }
        _player.Attack(Vector2.right);
    }

    private void _Throw(InputAction.CallbackContext cc) {
        if (!_setupThrow) { return; }
        _player.Throw(_player.Orientation);
    }

    private void _SetupThrow(InputAction.CallbackContext cc) {
        _setupThrow = cc.ReadValue<float>() != 0f;
    }
}
