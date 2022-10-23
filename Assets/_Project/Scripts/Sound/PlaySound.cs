using System.Collections;
using UnityEngine;

//[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour {

    public AudioSource audioSource;
    public Sound sound;

    //private void LateUpdate() {
    //    if (audioSource.isPlaying && !sound.loop) {
    //        SoundManager.Instance.BackToPool(gameObject);
    //    }
    //}

    public void Init() {
        if (!audioSource)
            audioSource = GetComponent<AudioSource>();
    }

    public IEnumerator StartSound() {
        audioSource.Play();
        yield return new WaitWhile(() => audioSource.isPlaying);
        SoundManager.Instance.BackToPool(gameObject);

        //do something
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