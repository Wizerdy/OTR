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

    [SerializeField] int _audioSourceNumber = 20;

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
            audioSourcesActive = new List<GameObject>();
            audioSourcesStandBy = new List<GameObject>();
        }

        for (int i = 0; i < _audioSourceNumber; i++) {
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

    public PlaySound PlaySfx(AudioName audioName) {
        return PlaySound(FindSfx(audioName));
    }

    public PlaySound PlayMusic(AudioName audioName) {
        return PlaySound(FindMusic(audioName));
    }

    private PlaySound PlaySound(Sound sound) {
        GameObject obj;
        if (audioSourcesStandBy.Count > 0) {
            obj = audioSourcesStandBy[0];
        } else {
            obj = CreateNewAudioSource();
        }

        PlaySound source = obj.GetComponent<PlaySound>();

        source.audioSource.pitch = 1f;
        if (sound.randomPitch) {
            source.audioSource.pitch = Random.Range(sound.minPitch, sound.maxPitch);
        }

        source.sound = sound;
        source.audioSource.clip = sound.audio[Random.Range(0, source.sound.audio.Length - 1)];
        source.audioSource.loop = sound.loop;
        source.audioSource.volume = sound.volume;

        obj.SetActive(true);
        source.StartSound();
        audioSourcesActive.Add(obj);
        audioSourcesStandBy.Remove(obj);
        source.OnEnd += (PlaySound component) => BackToPool(component.gameObject);

        return source;
    }

    public PlaySound PlaySfxByIndex(int index) {
        if (index >= soundsSfx.Length) { Debug.LogError("SFX index overflow"); return null; }
        if (soundsSfx[index] != null)
            return PlaySfx(soundsSfx[index].audioName);
        else
            Debug.LogError("NO SFX FROM THIS INDEX");
        return null;
    }

    public PlaySound PlayMusicByIndex(int index) {
        if (index >= soundsSfx.Length) { Debug.LogError("Music index overflow"); return null; }
        if (soundsMusic[index] != null)
            PlayMusic(soundsMusic[index].audioName);
        else
            Debug.LogError("NO MUSIC FROM THIS INDEX");
        return null;
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

    public void StopAudioWithReduceVolume(AudioName audioName) {
        if (audioSourcesActive.Count > 0) {
            foreach (var obj in audioSourcesActive) {
                var playedSounce = obj.GetComponent<PlaySound>();
                if (playedSounce.sound.audioName == audioName) {
                    while (playedSounce.audioSource.volume > 0.1f)
                        playedSounce.audioSource.volume -= Time.deltaTime;
                    
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