using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using TMPro;

public class UIMultipleHealthText : MonoBehaviour {
    [SerializeField] HealthReference _health;
    [SerializeField] TextMeshProUGUI _text = null;

    [Header("Value")]
    [SerializeField] int _index = 0;
    [SerializeField] int _maxIndex = 10;

    void Start() {
        ChangeText(_maxIndex - _index);
        if (!_health.IsValid()) { return; }
        _health.Instance.OnDeath += _ChangeUI;
    }

    void _ChangeUI() {
        ++_index;
        ChangeText(_maxIndex - _index);
    }

    void ChangeText(int index) {
        if (_text == null) { return; }
        _text.text = "x" + index;
    }
}
