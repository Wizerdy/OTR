using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedOderInLayer : MonoBehaviour {
    [SerializeField] Renderer _target;
    [SerializeField] int _difference = 1;

    Renderer _renderer;

    private void Start() {
        _renderer = GetComponent<Renderer>();
    }

    void Update() {
        _renderer.sortingOrder = _target.sortingOrder + _difference;
    }
}
