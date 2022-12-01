using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class TestUser : MonoBehaviour {
    List<InputUser> _users;

    void Start() {
        InputUser.onChange += _OnChange;
        UserCreator.ClearUsers();
    }

    void _OnChange(InputUser user, InputUserChange change, InputDevice device) {
        switch (change) {
            case InputUserChange.Added:

                break;
        }
    }

    void Update() {

    }
}
