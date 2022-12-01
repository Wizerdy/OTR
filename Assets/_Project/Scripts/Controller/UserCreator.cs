using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class UserCreator : MonoBehaviour {
    [SerializeField] InputActionAsset _actions;

    [SerializeField, HideInInspector] static BetterEvent<InputUser, InputUserChange> _onChange = new BetterEvent<InputUser, InputUserChange>();

    //List<InputUser> _users = new List<InputUser>();

    public static event UnityAction<InputUser, InputUserChange> OnChange { add => _onChange += value; remove => _onChange -= value; }

    private void Start() {
        //LoadUsers(out _users);

        InputUser.listenForUnpairedDeviceActivity = 1;
        InputUser.onUnpairedDeviceUsed += _OnUnpairedDeviceUsed;

        //InputUser.onChange += _OnChange;
    }

    private void OnDestroy() {
        InputUser.listenForUnpairedDeviceActivity = 0;
    }

    private void _OnUnpairedDeviceUsed(InputControl control, InputEventPtr ptr) {
        if (control.device is Mouse) { return; }

        CreateUser(control.device, _actions);
    }

    //private void _OnChange(InputUser user, InputUserChange change, InputDevice device) {
    //    switch (change) {
    //        case InputUserChange.Added:
    //        case InputUserChange.Removed:
    //            LoadUsers(out _users);
    //            break;
    //    }
    //}

    public static InputUser CreateUser(InputDevice device, InputActionAsset actions) {
        InputUser newUser = InputUser.CreateUserWithoutPairedDevices();
        PairToDevice(newUser, device, actions);
        Debug.Log("New User:" + newUser.index + " devices:" + Print(newUser.pairedDevices));

        _onChange.Invoke(newUser, InputUserChange.Added);
        return newUser;
    }

    public static void ClearUsers() {
        ReadOnlyArray<InputUser> users = InputUser.all;
        for (int i = 0; i < users.Count; i++) {
            DeleteUser(users[i]);
        }
    }

    public static void DeleteUser(InputUser user) {
        Debug.Log("Removed User:" + user.index + " devices:" + Print(user.pairedDevices));
        user.UnpairDevicesAndRemoveUser();
    }

    public static void PairToDevice(InputUser user, InputDevice device, InputActionAsset actions) {
        user.UnpairDevices();
        string controlScheme = "Gamepad";
        if (device is Mouse || device is Keyboard) {
            InputUser.PerformPairingWithDevice(Keyboard.current, user);
            InputUser.PerformPairingWithDevice(Mouse.current, user);
            controlScheme = "Keyboard";
        } else {
            InputUser.PerformPairingWithDevice(device, user);
        }

        user.AssociateActionsWithUser(Instantiate(actions));
        user.ActivateControlScheme(controlScheme);
    }

    public static void LoadUsers(out List<InputUser> userList) {
        ReadOnlyArray<InputUser> users = InputUser.all;
        userList = new List<InputUser>();
        for (int i = 0; i < users.Count; i++) {
            userList.Add(users[i]);
        }
    }

    //void DebugUsers() {
    //    for (int i = 0; i < _users.Count; i++) {
    //        Debug.Log(_users[i].id + " " + _users[i].index + " d:" + _users[i].pairedDevices[0]);
    //    }
    //}

    public static string Print<T>(ReadOnlyArray<T> array) {
        string output = "";
        for (int i = 0; i < array.Count; i++) {
            output += "[" + array[i].ToString() + "]";
        }
        return output;
    }
}
