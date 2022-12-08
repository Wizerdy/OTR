using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;

public class CharacterLoader : MonoBehaviour {
    [SerializeField] InputUserController.PlayerController _controller;
    [SerializeField] CharacterData _characterData;

    private void Start() {
        if (_characterData == null) { return; }
        _characterData.OnNewUser += _NewUser;
        if (!_characterData.User.valid) { return; }
        _controller.AssignUser(_characterData.User);
    }

    private void _NewUser(InputUser user) {
        _controller.AssignUser(user);
    }
}
