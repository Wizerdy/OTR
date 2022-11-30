using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;
using ToolsBoxEngine;

namespace InputUserController {
    public class PlayerController : MonoBehaviour {
        [SerializeField] InputUser _user;
        [SerializeField] PlayerEntity _player;
        [SerializeField] AimHelperReference _aimHelperReference;

        InputActionAsset _inputs;

        bool _aiming = false;
        bool _aimLine = false;

        void Start() {
            AssignActions(_inputs);
            _player.ShowAimLine(false);
        }

        private void Update() {
            Vector2 direction = _inputs?["Movements"].ReadValue<Vector2>() ?? Vector2.zero;
            if (direction != Vector2.zero) { direction = direction.normalized; }
            _player.Move(direction);
            if (!_aiming) { _player.Aim(direction, false); }
        }

        private void OnDestroy() {
            UnassignActions(_inputs);
        }

        private void AssignActions(InputActionAsset actions) {
            if (actions == null) { return; }

            actions.FindActionMap("Gameplay").Enable();
            actions["Aim"].performed += _Aim;
            actions["Aim"].canceled += _StopAim;
            actions["First Attack"].performed += _PressFirstAttack;
            actions["First Attack"].canceled += _PressFirstAttackEnd;
            actions["Second Attack"].performed += _PressSecondAttack;
            actions["Second Attack"].canceled += _PressSecondAttackEnd;
            actions["Throw"].performed += _SetupThrow;
            actions["Throw"].canceled += _Throw;
            actions["Teleport"].performed += _Teleport;

            //_playerInput.actions["SetupThrow"].performed += _SetupThrow;
            //_playerInput.actions["SetupThrow"].canceled += _SetupThrow;
        }

        private void UnassignActions(InputActionAsset actions) {
            if (actions == null) { return; }

            actions.FindActionMap("Gameplay").Disable();
            actions["Aim"].performed -= _Aim;
            actions["Aim"].canceled -= _StopAim;
            actions["First Attack"].performed -= _PressFirstAttack;
            actions["First Attack"].canceled -= _PressFirstAttackEnd;
            actions["Second Attack"].performed -= _PressSecondAttack;
            actions["Second Attack"].canceled -= _PressSecondAttackEnd;
            actions["Throw"].performed -= _SetupThrow;
            actions["Throw"].canceled -= _Throw;
            actions["Teleport"].performed -= _Teleport;

            //_playerInput.actions["SetupThrow"].performed += _SetupThrow;
            //_playerInput.actions["SetupThrow"].canceled += _SetupThrow;
        }

        public void AssignUser(InputUser user) {
            UnassignActions(_inputs);
            _user = user;
            _inputs = (InputActionAsset)_user.actions;
            AssignActions(_inputs);
        }

        #region Actions

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

        #endregion
    }
}