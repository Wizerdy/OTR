using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SetAudio : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private AudioMixer mixer;

    private void Start() {
        slider.value = PlayerPrefs.GetFloat("MasterVolume", 0.80f);    
    }

    public void SetVolumeLevel(float sliderValue) {
        mixer.SetFloat("MasterVolume", Mathf.Log10(sliderValue) * 20);
        PlayerPrefs.SetFloat("MasterVolume", sliderValue);
    }
}
