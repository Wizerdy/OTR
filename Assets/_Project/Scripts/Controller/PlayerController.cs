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
        _playerInput.actions["FirstAttack"].performed += _FirstAttack;
        _playerInput.actions["FirstAttack"].canceled += _FirstAttackEnd;
        _playerInput.actions["SecondAttack"].performed += _SecondAttack;
        _playerInput.actions["SecondAttack"].canceled += _SecondAttackEnd;
        _playerInput.actions["Throw"].performed += _Throw;

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

    private void _FirstAttack(InputAction.CallbackContext cc) {
        _player.Attack(AttackIndex.FIRST, _player.Orientation);
    }

    private void _FirstAttackEnd(InputAction.CallbackContext cc) {
        _player.AttackEnd(AttackIndex.FIRST);
    }

    private void _SecondAttack(InputAction.CallbackContext cc) {
        _player.Attack(AttackIndex.SECOND, _player.Orientation);
    }

    private void _SecondAttackEnd(InputAction.CallbackContext cc) {
        _player.AttackEnd(AttackIndex.SECOND);
    }

    private void _Throw(InputAction.CallbackContext cc) {
        if (_player.HasWeapon) {
            _player.Throw(_player.Orientation);
        } else {
            _player.Dash(_player.Orientation);
        }
    }
}
