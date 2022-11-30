using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject optionScreen;

    public void BackToMainScreen() {
        optionScreen.SetActive(false);
        mainScreen.SetActive(true);
    }

    public void GoToOption() {
        mainScreen.SetActive(false);
        optionScreen.SetActive(true);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
