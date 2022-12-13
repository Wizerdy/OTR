using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour {
    [SerializeField] string loadSceneName;

    AsyncOperation _operation;

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player") {
            Load(loadSceneName);
        }
    }

    public void Load(string scene) {
        if (scene.Equals("")) { scene = loadSceneName; }
        SceneManager.LoadScene(scene, LoadSceneMode.Single);
    }

    public void LoadAsync(string scene) {
        if (scene.Equals("")) { scene = loadSceneName; }
        _operation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
        _operation.allowSceneActivation = false;
    }

    public void ChangeToAsync() {
        _operation.allowSceneActivation = true;
    }
}
