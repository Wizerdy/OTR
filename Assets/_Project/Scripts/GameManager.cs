using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int deadPlayersIndex;
    public GameObject defeatScreen;
    public GameObject winScreen;
    float _timer = 0f;

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

    private void Update() {
        if (Time.deltaTime > 0.5f) { return; }
        _timer += Time.deltaTime;
    }

    public void OnPlayerDeath() {
        deadPlayersIndex++;
        if (deadPlayersIndex >= 3) {
            defeatScreen.SetActive(true);
            deadPlayersIndex = 0;
        }
    }

    public void Win() {
        winScreen.SetActive(true);
        winScreen.GetComponent<UITimer>()?.SetTimer(_timer);
    }

    public void OnPlayerRevive() {
        if (deadPlayersIndex <= 0) {
            return;
        }
        deadPlayersIndex--;
    }
}
