using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIntroThenLoop : MonoBehaviour
{
    Sound introMusic;
    float introDuration;
    float duration;
    bool looping;

    public void PlayIntro() {
        SoundManager.Instance.PlayMusic(AudioName.IntroMusic);
    }

    public void PlayLoop() {
        SoundManager.Instance.PlayMusic(AudioName.LoopMusic);
    }

    // Start is called before the first frame update
    void Start()
    {
        introMusic = SoundManager.Instance.GetMusic(AudioName.IntroMusic);
        introDuration = introMusic.audio[0].samples / 49976.1f;
        duration = 0.0f;
        looping = false;
        PlayIntro();
    }

    // Update is called once per frame
    void Update()
    {
        if (looping == false && introDuration < duration) {
            PlayLoop();
            looping = true;
        } else if (looping == false) {
            duration += Time.deltaTime;
        }
        Debug.Log(introDuration + " . " + duration);
    }
}
