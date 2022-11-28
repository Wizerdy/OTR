using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFullscreen : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    public void ToggleFullScreen() {
        if (toggle.isOn)
            Screen.fullScreen = Screen.fullScreen;
        else
            Screen.fullScreen = !Screen.fullScreen;
    }

}
