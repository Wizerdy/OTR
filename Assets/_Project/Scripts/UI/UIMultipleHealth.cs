using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ToolsBoxEngine;
using TMPro;

public class UIMultipleHealth : MonoBehaviour {
    [SerializeField] HealthReference _health;
    [SerializeField] Image _image = null;
    [SerializeField] TextMeshProUGUI _text = null;

    [Header("Value")]
    [SerializeField] int _index = 0;
    [SerializeField] List<Color> _colors;

    void Start() {
        ChangeColor(_index);
        ChangeText(_colors.Count - _index - 1);
        if (!_health.IsValid()) { return; }
        _health.Instance.OnDeath += _ChangeUI;
    }

    void _ChangeUI() {
        ++_index;
        ChangeColor(_index);
        ChangeText(_colors.Count - _index - 1);
    }

    void ChangeColor(int index) {
        _image.color = index < _colors.Count ? _colors[index] : Color.white;
    }

    void ChangeText(int index) {
        if (_text == null) { return; }
        _text.text = "x" + index;
    }
}
