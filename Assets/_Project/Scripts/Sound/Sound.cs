using UnityEngine;

public enum AudioName {
    None,
    ShieldSlash,
    ShieldSlashHit,
    ShieldParry,
    IntroMusic,
    LyreShooting,
    BloodFistHit,
    AxeShieldHit,
    PlayerDeath,
    PlayerTakeDamage,
    PlayerGrabWeapon,
    PlayerThrowWeapon,
    PlayerWalk,
    UI_Back,
    UI_Select,
    UI_Click,
    UI_Validate,
    UI_StartGame,
    ShieldPerfectParry,
    LyreBoost,
    BloodFistHeal,
    BossHit,
    BossDeath,
    LoopMusic
}

[System.Serializable]
public class Sound {
    public AudioName audioName;
    public AudioClip[] audio;
    [Range(0.0f, 1.0f)]
    public float volume = 1.0f;
    public bool loop;
    public bool randomPitch;
    public float minPitch;
    public float maxPitch;
    //public bool playOnAwake;
}