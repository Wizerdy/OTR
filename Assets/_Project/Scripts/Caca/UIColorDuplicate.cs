using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIColorDuplicate : MonoBehaviour {
    [SerializeField] Graphic _us;
    [SerializeField] Graphic _target;

    [SerializeField, Range(0f, 1f)] float _alpha = 1f;

    private void Reset() {
        _us = GetComponent<Graphic>();
    }

    void Update() {
        if (_target == null || _us == null) { return; }
        Color color = _target.color;
        color.a = _alpha;
        _us.color = color;
    }
}
