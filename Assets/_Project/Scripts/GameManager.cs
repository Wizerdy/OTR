using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int deadPlayersIndex;
    public GameObject defeatScreen;

    private void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }
    }

    private void Start() {
        deadPlayersIndex = 0;
    }

    public void OnPlayerDeath() {
        deadPlayersIndex++;
        if (deadPlayersIndex >= 3) {
            defeatScreen.SetActive(true);
            deadPlayersIndex = 0;
        }
    }

    public void OnPlayerRevive() {
        if (deadPlayersIndex <= 0) {
            return;
        }
        deadPlayersIndex--;
    }
}
