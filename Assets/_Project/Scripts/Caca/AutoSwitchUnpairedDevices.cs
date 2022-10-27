using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.LowLevel;
using ToolsBoxEngine;

public class AutoSwitchUnpairedDevices : MonoBehaviour {
    [SerializeField] PlayerInput _input;

    private void Start() {
        InputUser.listenForUnpairedDeviceActivity = 1;
        InputUser.onUnpairedDeviceUsed += (InputControl control, InputEventPtr ptr) => PairToDevice(_input, control.device);
    }

    void PairToDevice(PlayerInput input, InputDevice device) {
        input.user.UnpairDevices();
        string controlScheme = "Gamepad";
        if (device is Mouse || device is Keyboard) {
            InputUser.PerformPairingWithDevice(Keyboard.current, input.user);
            InputUser.PerformPairingWithDevice(Mouse.current, input.user);
            controlScheme = "Keyboard";
        } else {
            InputUser.PerformPairingWithDevice(device, input.user);
        }

        input.user.AssociateActionsWithUser(input.actions);
        input.user.ActivateControlScheme(controlScheme);
    }
}
