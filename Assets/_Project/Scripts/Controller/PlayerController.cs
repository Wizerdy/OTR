using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ToolsBoxEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] PlayerInput _playerInput;
    [SerializeField] PlayerEntity _player;
    [SerializeField] AimHelperReference _aimHelperReference;

    bool _setupThrow = false;

    private void Reset() {
        _playerInput = GetComponent<PlayerInput>();
    }

    void Start() {
        _playerInput.actions["Aim"].performed += _Aim;
        _playerInput.actions["Aim"].canceled += _StopAim;
        _playerInput.actions["Attack"].performed += _Attack;
        _playerInput.actions["Attack"].canceled += _AttackEnd;
        _playerInput.actions["Throw"].performed += _Throw;
        _playerInput.actions["Catch"].performed += _Catch;

        //_playerInput.actions["SetupThrow"].performed += _SetupThrow;
        //_playerInput.actions["SetupThrow"].canceled += _SetupThrow;
    }

    private void Update() {
        _player.Move(_playerInput.actions["Movements"].ReadValue<Vector2>());
    }

    private void _Aim(InputAction.CallbackContext cc) {
        Vector2 direction = cc.ReadValue<Vector2>();
        if (_aimHelperReference.Valid()) { direction = _aimHelperReference.Instance.Aim(transform.Position2D(), direction); }
        _player.Aim(direction);
    }

    private void _StopAim(InputAction.CallbackContext cc) {
        _player.Aim(Vector2.zero);
    }

    private void _Attack(InputAction.CallbackContext cc) {
        if (_setupThrow) { return; }
        _player.Attack(_player.Orientation);
    }

    private void _AttackEnd(InputAction.CallbackContext cc) {
        _player.AttackEnd();
    }

    private void _Throw(InputAction.CallbackContext cc) {
        //if (!_setupThrow) { return; }
        _player.Throw(_player.Orientation);
    }
    
    private void _Catch(InputAction.CallbackContext cc) {
        _player.TryCatch();
    }

    //private void _SetupThrow(InputAction.CallbackContext cc) {
    //    _setupThrow = cc.ReadValue<float>() != 0f;
    //}
}
