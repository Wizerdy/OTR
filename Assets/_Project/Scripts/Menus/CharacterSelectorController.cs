using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using ToolsBoxEngine.BetterEvents;

public class CharacterSelectorController : MonoBehaviour {
    [SerializeField] CharacterSelectorCanvas _canvas;
    //[SerializeField, HideInInspector] BetterEvent<int> _onConfirm = new BetterEvent<int>();

    static List<int> _taken = new List<int>();

    InputUser _user;
    InputActionAsset _inputs;

    public bool HasUser => _inputs != null;
    public bool Locked => _canvas.Locked;
    //public event UnityAction<int> OnConfirm { add => _onConfirm += value; remove => _onConfirm -= value; }

    private void Reset() {
        _canvas = GetComponent<CharacterSelectorCanvas>();
    }

    public void UserJoin(InputUser user) {
        _user = user;
        _inputs = ((InputActionAsset)user.actions);
        AssignActions(_inputs);
        _inputs.FindActionMap("Character Selection").Enable();
        Select(1);
    }

    public void UserLeave() {
        _inputs = null;
        UnassignActions(_inputs);
        UserCreator.DeleteUser(_user);
        _canvas.Join(0);
    }

    private void AssignActions(InputActionAsset actions) {
        if (actions == null) { return; }

        actions["Selection"].started += _OnSelection;
        actions["Confirm"].started += _OnConfirm;
        actions["Leave"].started += _OnLeave;
    }

    private void UnassignActions(InputActionAsset actions) {
        if (actions == null) { return; }

        actions["Selection"].started -= _OnSelection;
        actions["Confirm"].started -= _OnConfirm;
        actions["Leave"].started -= _OnLeave;
    }

    private void _OnLeave(InputAction.CallbackContext obj) {
        if (Locked) {
            _canvas.Selected(false);
            _taken.Remove(_canvas.CurrentIndex);
            return;
        }

        UserLeave();
    }

    private void _OnConfirm(InputAction.CallbackContext obj) {
        Debug.Log("- Input:" + _user.index);
        if (_canvas.CurrentIndex == 0) { return; }
        if (_canvas.Locked) { return; }
        if (_taken.Contains(_canvas.CurrentIndex)) { return; }

        _canvas.Selected(true);
        _taken.Add(_canvas.CurrentIndex);
        //_onConfirm.Invoke(_canvas.CurrentIndex);
    }

    private void _OnSelection(InputAction.CallbackContext obj) {
        if (_canvas.Locked) { return; }

        Switch(obj.ReadValue<float>() >= 1);
    }

    private void Switch(bool right) {
        int index = _canvas.CurrentIndex;
        index += right ? 1 : -1;
        index %= 4;

        if (index == 0) {
            index = right ? 1 : 3;
        }
        Select(index);
    }

    private void Select(int index) {
        _canvas.Join(index);
        _canvas.Taken(_taken.Contains(_canvas.CurrentIndex));
    }
}
