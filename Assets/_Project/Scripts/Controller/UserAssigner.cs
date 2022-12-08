using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class UserAssigner : MonoBehaviour {
    [SerializeField] List<CharacterData> _datas;

    int _currentIndex = 0;

    void Start() {
        UserCreator.OnChange += _InputUserChange;
    }

    private void _InputUserChange(InputUser user, InputUserChange change) {
        switch (change) {
            case InputUserChange.Added:
                AssignUser(_datas[_currentIndex], user);
                ++_currentIndex;
                _currentIndex %= _datas.Count;
                break;
        }
    }

    public void AssignUser(CharacterData data, InputUser user) {
        Debug.Log(user + " .. " + data.Name);
        data.User = user;
    }
}
