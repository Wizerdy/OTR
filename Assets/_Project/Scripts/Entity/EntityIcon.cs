using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityIcon : MonoBehaviour, IEntityAbility {
    [SerializeField] private SpriteRenderer cross;
    [SerializeField] private SpriteRenderer crossHighlight;

    public void ShowCross() {
        cross.enabled = true;
    }

    public void ShowCrossHighlight() {
        crossHighlight.enabled = true;
    }

    public void HideCross() {
        cross.enabled = false;
    }

    public void HideCrossHighlight() {
        crossHighlight.enabled = false;
    }

}
