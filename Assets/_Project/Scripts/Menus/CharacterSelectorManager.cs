using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using ToolsBoxEngine.BetterEvents;

public class CharacterSelectorManager : MonoBehaviour {
    [SerializeField] BetterEvent _onReady = new BetterEvent();

    CharacterSelectorController[] _controllers;
    CharacterSelectorCanvas[] _canvas;

    public event UnityAction OnReady { add => _onReady += value; remove => _onReady -= value; }

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
            _onReady.Invoke();
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
