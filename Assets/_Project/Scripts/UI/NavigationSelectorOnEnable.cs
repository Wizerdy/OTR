using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class NavigationSelectorOnEnable : MonoBehaviour {
    [SerializeField] GameObject _enableTarget;
    [SerializeField] GameObject _disableTarget;

    private void OnEnable() {
        EventSystem.current.SetSelectedGameObject(_enableTarget);
    }

    private void OnDisable() {
        EventSystem.current.SetSelectedGameObject(_disableTarget);
    }
}
