using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class CharacterSelectorManager : MonoBehaviour {
    CharacterSelectorController[] _controllers;
    CharacterSelectorCanvas[] _canvas;

    void Start() {
        UserCreator.OnChange += _OnChange;
        UserCreator.ClearUsers();

        _controllers = GetComponentsInChildren<CharacterSelectorController>();
        _canvas = GetComponentsInChildren<CharacterSelectorCanvas>();
    }

    void _OnChange(InputUser user, InputUserChange change) {
        switch (change) {
            case InputUserChange.Added:
                NewUser(user);
                break;
        }
    }

    private void NewUser(InputUser user) {
        for (int i = 0; i < _controllers.Length; i++) {
            if (!_controllers[i].HasUser) {
                _controllers[i].UserJoin(user);
                return;
            }
        }
        UserCreator.DeleteUser(user);
    }

    private void Update() {
        if (Ready()) {

        }
    }

    public bool Ready() {
        for (int i = 0; i < _controllers.Length; i++) {
            if (!_controllers[i].Locked && !_controllers[i].HasUser) {
                return false;
            }
        }
        return true;
    }
}
