using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuDoor : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Start() {
        animator.enabled = false;
    }

    public void OnDoorOpen() {
        Debug.Log("next scene");
    }
}
