using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Users;

public class CharacterLoader : MonoBehaviour {
    [SerializeField] InputUserController.PlayerController _controller;
    [SerializeField] CharacterData _characterData;

    private void Start() {
        if (_controller == null) { return; }
        if (_characterData == null) { return; }
        _characterData.OnNewUser += _NewUser;
        if (_characterData.User.valid) {
            _controller.AssignUser(_characterData.User);
        } else if (_characterData.UserId != null) {
            Debug.Log("User not valid, using id : " + _characterData.UserId.Value);
            _controller.AssignUser(InputUser.all[(int)_characterData.UserId.Value - 1]);
        }
    }

    private void _NewUser(InputUser user) {
        _controller.AssignUser(user);
    }
}
