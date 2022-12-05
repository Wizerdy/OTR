using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private EventSystem eventSystem;
    [SerializeField] private GameObject mainScreen;
    [SerializeField] private GameObject optionScreen;
    [SerializeField] private GameObject characterSelection;
    [SerializeField] private Animator doorAnimator;

    public void StartGame() {
        doorAnimator.enabled = true;
    }
    public void GoToOption(GameObject objToSelect) {
        optionScreen.SetActive(true);
        eventSystem.SetSelectedGameObject(null);

        eventSystem.SetSelectedGameObject(objToSelect);
    }

    public void GoToCharacterSelection(GameObject objToSelect) {
        characterSelection.SetActive(true);
        eventSystem.SetSelectedGameObject(null);

        eventSystem.SetSelectedGameObject(objToSelect);
    }


    public void BackToMainScreen(GameObject objToSelect) {
        optionScreen.SetActive(false);
        characterSelection.SetActive(false);
        mainScreen.SetActive(true);

        eventSystem.SetSelectedGameObject(null);
        eventSystem.SetSelectedGameObject(objToSelect);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
