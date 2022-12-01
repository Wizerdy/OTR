using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class ColorHolePositionSetter : MonoBehaviour {
    [SerializeField] TransformReference _target;

    SpriteRenderer _renderer;

    void Start() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Update() {
        if (!_target.IsValid()) { return; }
        _renderer.material?.SetVector("_Position", _target.Instance.position);
    }
}
