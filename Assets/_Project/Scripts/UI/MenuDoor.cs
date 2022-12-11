using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDoor : MonoBehaviour {
    [SerializeField] private string levelToLoad = "TheOne";
    [SerializeField] private Animator animator;

    AsyncOperation _loadScene;

    private void Start() {
        animator.enabled = false;
    }

    public void OnDoorOpen() {
        _loadScene.allowSceneActivation = true;
    }

    public void OnAnimationStart() {
        _loadScene = SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Single);
        _loadScene.allowSceneActivation = false;
    }
}
