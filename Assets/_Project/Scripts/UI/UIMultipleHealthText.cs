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
    [SerializeField] private float scaleDownValue = 1.5f;

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
        StartCoroutine(StretchNumber());
    }

    private IEnumerator StretchNumber() {
        _text.gameObject.transform.localScale = new Vector3(scaleDownValue, scaleDownValue, 1);

        for (float i = scaleDownValue; i > 1; i -= Time.deltaTime) {
            _text.gameObject.transform.localScale = new Vector3(i, i, 1);
            yield return null;
        }
    }
}
