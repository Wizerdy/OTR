using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnNewPhase : MonoBehaviour {
    private void Start() {
        DestroyerOnNewPhase._instance.Add(this);
    }

    private void OnDestroy() {
        DestroyerOnNewPhase._instance.Remove(this);
    }
}
