using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetFullscreen : MonoBehaviour
{
    [SerializeField] private Toggle toggle;

    public void SetScreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
    }

}
