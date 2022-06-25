using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {
    [SerializeField] PlayerInput _playerInput;

    PlayerController _playerController;

    private void Reset() {
        _playerInput = GetComponent<PlayerInput>();
    }

    void Start() {
        _playerController = new PlayerController();

        
    }

    private void _Move(InputAction.CallbackContext cc) {
        cc.ReadValue<Vector2>();
    }
}
