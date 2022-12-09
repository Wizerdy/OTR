using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Audio;
using ToolsBoxEngine.BetterEvents;

//[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {
    public AudioSource audioSource;
    public Sound sound;
    [SerializeField, HideInInspector] BetterEvent<PlaySound> _onStart = new BetterEvent<PlaySound>();
    [SerializeField, HideInInspector] BetterEvent<PlaySound> _onEnd = new BetterEvent<PlaySound>();

    Coroutine _soundRoutine = null;
    bool _playing = false;

    public bool Playing => _playing;
    public event UnityAction<PlaySound> OnStart { add => _onStart += value; remove => _onStart -= value; }
    public event UnityAction<PlaySound> OnEnd { add => _onEnd += value; remove => _onEnd -= value; }

    //private void LateUpdate() {
    //    if (audioSource.isPlaying && !sound.loop) {
    //        SoundManager.Instance.BackToPool(gameObject);
    //    }
    //}

    public void Init(AudioMixerGroup mixer) {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();

        audioSource.outputAudioMixerGroup = mixer;
    }

    public void StartSound() {
        if (_playing) { StopSound(); }
        _soundRoutine = StartCoroutine(IStartSound());
    }

    public void StopSound() {
        if (_soundRoutine != null) { StopCoroutine(_soundRoutine); }

        audioSource.Stop();
        _playing = false;
        _onEnd.Invoke(this);
        _onEnd.ClearListener();
        _onStart.ClearListener();
    }

    public IEnumerator IStartSound() {
        audioSource.Play();
        _onStart.Invoke(this);
        _playing = true;
        yield return new WaitWhile(() => audioSource.isPlaying);
        StopSound();
    }

    //[HideInInspector]
    //public bool declencherAudio = false;

    //[SerializeField]
    //private bool isMusique = true;

    //[SerializeField]
    //private AudioName audioName;

    //private SoundManager soundManager;
    //private Sound soundToPlay;
    //private AudioSource audioSource;

    //// Start is called before the first frame update
    //void Start() {
    //    soundManager = SoundManager.Instance;
    //    audioSource = GetComponent<AudioSource>();

    //    if (isMusique) {
    //        soundToPlay = GetAudio(audioName, soundManager.soundsMusic);
    //    } else {
    //        soundToPlay = GetAudio(audioName, soundManager.soundsSfx);
    //    }

    //    if (soundToPlay != null) {
    //        audioSource.clip = soundToPlay.audio;
    //        audioSource.loop = soundToPlay.loop;
    //        audioSource.volume = soundToPlay.volume;
    //        if (soundToPlay.playOnAwake) {
    //            PlayAudio();
    //        }
    //    } else {
    //        Debug.LogError("DIDNT FIND AUDIO IN MANAGER");
    //    }

    //}

    //private void Update() {
    //    if ((isMusique && !soundManager.activeMusic) || (!isMusique && !soundManager.activeSfx)) {
    //        audioSource.mute = true;
    //    } else {
    //        audioSource.mute = false;
    //    }
    //}

    //public Sound GetAudio(AudioName audioName, Sound[] allSounds) {
    //    foreach (Sound soundSelected in allSounds) {
    //        if (soundSelected.audioName == audioName) {
    //            return soundSelected;
    //        }
    //    }
    //    return null;
    //}

    //public void PlayAudio() {
    //    audioSource.Play();
    //}

    //public void StopAudio() {
    //    audioSource.Stop();
    //}

}