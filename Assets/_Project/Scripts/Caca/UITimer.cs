using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UITimer : MonoBehaviour {
    [SerializeField] TextMeshProUGUI _text;

    public void SetTimer(float time) {
        Debug.Log("Timer " + time);
        string text = "";
        text += Mathf.FloorToInt(time/60f);
        if (text.Length < 2) { text = "0" + text; }
        text += ":";
        string seconds = (Mathf.RoundToInt(time % 60)).ToString();
        if (seconds.Length < 2) { seconds = "0" + seconds; }
        text += seconds;

        _text.text = text;
    }
}
