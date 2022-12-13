using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ToolsBoxEngine;
using TMPro;

public class UIMultipleHealth : MonoBehaviour {
    [SerializeField] HealthReference _health;
    [SerializeField] Image _image = null;

    [Header("Value")]
    [SerializeField] int _index = 0;
    [SerializeField] List<Color> _colors;

    void Start() {
        ChangeColor(_index);
        if (!_health.IsValid()) { return; }
        _health.Instance.OnDeath += _ChangeUI;
    }

    void _ChangeUI() {
        ++_index;
        ChangeColor(_index);
    }

    void ChangeColor(int index) {
        _image.color = index < _colors.Count ? _colors[Mathf.Max(index, 0)] : Color.white;
    }
}
