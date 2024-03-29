using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using ToolsBoxEngine;

namespace PlayerInputController {
    public class PlayerController : MonoBehaviour {
        [SerializeField] PlayerInput _playerInput;
        [SerializeField] PlayerEntity _player;
        [SerializeField] AimHelperReference _aimHelperReference;

        bool _aiming = false;
        bool _aimLine = false;

        private void Reset() {
            _playerInput = GetComponent<PlayerInput>();
        }

        void Start() {
            _playerInput.actions["Aim"].performed += _Aim;
            _playerInput.actions["Aim"].canceled += _StopAim;
            _playerInput.actions["First Attack"].performed += _PressFirstAttack;
            _playerInput.actions["First Attack"].canceled += _PressFirstAttackEnd;
            _playerInput.actions["Second Attack"].performed += _PressSecondAttack;
            _playerInput.actions["Second Attack"].canceled += _PressSecondAttackEnd;
            _playerInput.actions["Throw"].performed += _SetupThrow;
            _playerInput.actions["Throw"].canceled += _Throw;
            _playerInput.actions["Teleport"].performed += _Teleport;
            _playerInput.actions["Revive"].performed += _Revive;

            _player.ShowAimLine(false);
            //_playerInput.actions["SetupThrow"].performed += _SetupThrow;
            //_playerInput.actions["SetupThrow"].canceled += _SetupThrow;
        }

        private void Update() {
            Vector2 direction = _playerInput.actions["Movements"].ReadValue<Vector2>();
            if (direction != Vector2.zero) { direction = direction.normalized; }
            _player.Move(direction);
            if (!_aiming) { _player.Aim(direction, false); }
        }

        private void _Aim(InputAction.CallbackContext cc) {
            Vector2 direction = cc.ReadValue<Vector2>();
            if (!_aiming && _player.HasWeapon && _player.Weapon.ShowAim) { _player.ShowAimLine(true); _aimLine = true; }
            if (_aimHelperReference.Valid()) { direction = _aimHelperReference.Instance.Aim(transform.Position2D(), direction); }
            _player.Aim(direction, true);
            _aiming = true;
        }

        private void _StopAim(InputAction.CallbackContext cc) {
            if (_aimLine) { _player.ShowAimLine(false); _aimLine = false; }
            _player.Aim(Vector2.zero, true);
            _aiming = false;
        }

        private void _PressFirstAttack(InputAction.CallbackContext cc) {
            _player.PressAttack(AttackIndex.FIRST, _player.Orientation);
        }

        private void _PressFirstAttackEnd(InputAction.CallbackContext cc) {
            _player.PressAttackEnd(AttackIndex.FIRST);
        }

        private void _PressSecondAttack(InputAction.CallbackContext cc) {
            _player.PressAttack(AttackIndex.SECOND, _player.Orientation);
        }

        private void _PressSecondAttackEnd(InputAction.CallbackContext cc) {
            _player.PressAttackEnd(AttackIndex.SECOND);
        }

        private void _SetupThrow(InputAction.CallbackContext cc) {
            if (_player.HasWeapon) {
                _player.ShowAimLine(true);
            } else {
                _player.Dash(_player.Orientation);
            }
        }

        private void _Teleport(InputAction.CallbackContext cc) {
            _player.transform.parent.transform.position = Vector3.zero;
        }

        private void _Throw(InputAction.CallbackContext cc) {
            if (_player.HasWeapon) {
                _player.ShowAimLine(false);
                _player.Throw(_player.Orientation);
            }
        }

        private void _Revive(InputAction.CallbackContext cc) {
            _player.TryRevive();
        }
    }
}