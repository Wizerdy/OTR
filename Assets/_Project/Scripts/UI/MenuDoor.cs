using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuDoor : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start() {
        animator.enabled = false;
    }

    public void OnDoorOpen() {
        SceneManager.LoadScene("TheOne");
    }
}
