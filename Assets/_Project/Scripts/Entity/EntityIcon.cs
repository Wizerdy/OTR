using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityIcon : MonoBehaviour, IEntityAbility {
    [SerializeField] private GameObject cross;
    [SerializeField] private GameObject crossHighlight;

    public void ShowCross() {
        cross.SetActive(true);
    }

    public void ShowCrossHighlight() {
        crossHighlight.SetActive(true);
    }

    public void HideCross() {
        cross.SetActive(false);
    }

    public void HideCrossHighlight() {
        crossHighlight.SetActive(false);
    }

}
