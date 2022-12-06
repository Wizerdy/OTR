using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour {
    public static SoundManager Instance { get; private set; }

    public Sound[] soundsMusic;
    public Sound[] soundsSfx;

    List<GameObject> audioSourcesStandBy = new List<GameObject>();
    List<GameObject> audioSourcesActive = new List<GameObject>();

    [HideInInspector]
    public bool activeMusic = true;
    [HideInInspector]
    public bool activeSfx = true;

    [SerializeField] private AudioMixerGroup mixer;

    //[SerializeField]
    //private AudioMixer mainMixerAudio;

    //[SerializeField]
    //private Slider sliderVolumeMusique;
    //[SerializeField]
    //private Slider sliderVolumeSfx;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }

        for (int i = 0; i < 15; i++) {
            audioSourcesStandBy.Add(CreateNewAudioSource());
        }
    }

    private void Start() {
        mixer.audioMixer.SetFloat("MasterVolume", Mathf.Log10(PlayerPrefs.GetFloat("MasterVolume", 0.80f)) * 20);
    }

    private Sound FindSfx(AudioName audioName) {
        foreach (Sound soundSelected in soundsSfx) {
            if (soundSelected.audioName == audioName) {
                return soundSelected;
            }
        }

        Debug.LogError("DIDNT FIND SFX IN MANAGER");
        return null;
    }

    private Sound FindMusic(AudioName audioName) {
        foreach (Sound soundSelected in soundsMusic) {
            if (soundSelected.audioName == audioName) {
                return soundSelected;
            }
        }

        Debug.LogError("DIDNT FIND MUSIC IN MANAGER");
        return null;
    }

    public void PlaySfx(AudioName audioName) {
        GameObject obj;
        if (audioSourcesStandBy[0] != null) {
            obj = audioSourcesStandBy[0];
            audioSourcesStandBy.Remove(audioSourcesStandBy[0]);
        } else {
            obj = CreateNewAudioSource();
        }

        audioSourcesActive.Add(obj);

        PlaySound source = obj.GetComponent<PlaySound>();
        Sound soundToPlay = FindSfx(audioName);

        if (soundToPlay.randomPitch) {
            source.audioSource.pitch = Random.Range(soundToPlay.minPitch, soundToPlay.maxPitch);
        }

        source.sound = soundToPlay;
        source.audioSource.clip = soundToPlay.audio[Random.Range(0, source.sound.audio.Length - 1)];
        source.audioSource.loop = soundToPlay.loop;
        source.audioSource.volume = soundToPlay.volume;
        //if (soundToPlay.playOnAwake) {
        obj.SetActive(true);
        StartCoroutine(source.StartSound());

        //source.audioSource.Play();
        //}
    }

    public void PlayMusic(AudioName audioName) {
        GameObject obj;
        if (audioSourcesStandBy[0] != null) {
            obj = audioSourcesStandBy[0];
            audioSourcesStandBy.Remove(audioSourcesStandBy[0]);
        } else {
            obj = CreateNewAudioSource();
        }

        audioSourcesActive.Add(obj);

        //AudioSource source = obj.GetComponent<AudioSource>();
        PlaySound source = obj.GetComponent<PlaySound>();
        Sound soundToPlay = FindMusic(audioName);

        if (soundToPlay.randomPitch) {
            source.audioSource.pitch = Random.Range(soundToPlay.minPitch, soundToPlay.maxPitch);
        }

        source.sound = soundToPlay;
        source.audioSource.clip = soundToPlay.audio[Random.Range(0, source.sound.audio.Length - 1)];
        source.audioSource.loop = soundToPlay.loop;
        source.audioSource.volume = soundToPlay.volume;

        //if (soundToPlay.playOnAwake) {
        obj.SetActive(true);
        StartCoroutine(source.StartSound());

        //source.audioSource.Play();
        //}
    }

    public void PlaySfxByIndex(int index) {
        if (soundsSfx[index] != null)
            PlaySfx(soundsSfx[index].audioName);
        else
            Debug.LogError("NO SFX FROM THIS INDEX");
    }

    public void PlayMusicByIndex(int index) {
        if (soundsMusic[index] != null)
            PlayMusic(soundsMusic[index].audioName);
        else
            Debug.LogError("NO MUSIC FROM THIS INDEX");
    }

    public void StopAudio(AudioName audioName) {
        if (audioSourcesActive.Count > 0) {
            foreach (var obj in audioSourcesActive) {
                var playedSounce = obj.GetComponent<PlaySound>();
                if (playedSounce.sound.audioName == audioName) {
                    playedSounce.audioSource.Stop();
                    BackToPool(obj);
                    return;
                }
            }
        } else {
            Debug.LogError("NO AUDIO SOURCE ACTIVE");
        }

    }

    public Sound GetMusic(AudioName name) {
        return FindMusic(name);
    }

    //public void SwitchActiveMusique() {
    //    activeMusic = !activeMusic;
    //}

    //public void SetVolumeMusique() {
    //    mainMixerAudio.SetFloat("VolumeMusique", sliderVolumeMusique.value);
    //}

    //public void SwitchActiveSfx() {
    //    activeSfx = !activeSfx;
    //}

    private GameObject CreateNewAudioSource() {
        GameObject source = new GameObject();
        source.transform.parent = transform;
        source.AddComponent<AudioSource>();
        var play = source.AddComponent<PlaySound>();
        play.Init(mixer);

        source.SetActive(false);

        return source;
    }

    public void BackToPool(GameObject obj) {
        audioSourcesActive.Remove(obj);
        obj.SetActive(false);
        audioSourcesStandBy.Add(obj);
    }
}