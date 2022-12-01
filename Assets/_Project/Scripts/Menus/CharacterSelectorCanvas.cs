using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterSelectorCanvas : MonoBehaviour {
    //[SerializeField] List<Character>
    [SerializeField] Animator _animator;
    [SerializeField] TextMeshProUGUI _characterName;

    int _colorIndex = 0;
    bool _selected = false;
    bool _taken = false;

    public int CurrentIndex => _colorIndex;
    public bool Locked => _selected;

    public void Join(int index) {
        _colorIndex = index;
        _animator.SetInteger("Index", index);
    }

    public void Selected(bool state) {
        _selected = state;
        _animator.SetBool("Confirmed", state);
    }

    public void Taken(bool state) {
        _taken = state;
        _animator.SetBool("Taken", state);
    }

    public void SetCharacterName(string text) {
        _characterName.text = text;
    }
}
