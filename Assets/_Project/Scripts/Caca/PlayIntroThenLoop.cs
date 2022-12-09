using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIntroThenLoop : MonoBehaviour {
    [SerializeField] AudioName _intro = AudioName.IntroMusic;
    [SerializeField] AudioName _loop = AudioName.LoopMusic;

    PlaySound _audioSource;

    //Sound introMusic;
    //float introDuration;
    //float duration;
    //bool looping;

    public PlaySound PlayIntro() {
        return SoundManager.Instance.PlayMusic(AudioName.IntroMusic);
    }

    public PlaySound PlayLoop() {
        return SoundManager.Instance.PlayMusic(AudioName.LoopMusic);
    }

    private void Start() {
        _audioSource = PlayIntro();
        _audioSource.OnEnd += _PlayLoop;
    }

    void _PlayLoop(PlaySound _) {
        PlayLoop();
    }

    //void Start()
    //{
    //    introMusic = SoundManager.Instance.GetMusic(AudioName.IntroMusic);
    //    introDuration = introMusic.audio[0].samples / 49976.1f;
    //    duration = 0.0f;
    //    looping = false;
    //    PlayIntro();
    //}

    //void Update()
    //{
    //    if (looping == false && introDuration < duration) {
    //        PlayLoop();
    //        looping = true;
    //    } else if (looping == false) {
    //        duration += Time.deltaTime;
    //    }
    //}
}
