using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLoader : MonoBehaviour {
    [SerializeField] InputUserController.PlayerController _controller;
    [SerializeField] CharacterData _characterData;
    [SerializeField] SpriteRenderer _renderer;

    private void Start() {
        if (_characterData != null) {
            _controller.AssignUser(_characterData.User);
        }
    }
}
