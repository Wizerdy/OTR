using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetResolution : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropDown;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Screen.currentResolution);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ResolutionChange() {
        string[] resolution = dropDown.options[dropDown.value].text.Split('x');
        Debug.Log(resolution[0]);
        Debug.Log(resolution[1]);

        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), Screen.fullScreen);
    }
}
