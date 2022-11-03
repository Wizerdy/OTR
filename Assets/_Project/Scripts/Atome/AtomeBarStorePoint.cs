using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class AtomeBarStorePoint : MonoBehaviour {
    [SerializeField] AtomeBar _atomeBar;
    [SerializeField] EntityStorePoint _entityStorePoint;

    private void Reset() {
        _atomeBar = GetComponent<AtomeBar>();
    }

    void Start() {
        if (_entityStorePoint == null) { return; }
        _entityStorePoint.CurrentValueChanged += (float value) => { if (value < _atomeBar.Value) { _atomeBar.Remove(Mathf.Abs(value - _atomeBar.Value), false); } else { _atomeBar.Add(Mathf.Abs(value - _atomeBar.Value)); } };
        _entityStorePoint.MaxValueChanged += (float value) => _atomeBar.ChangeMaxValue(value);
        _entityStorePoint.MinValueChanged += (float value) => _atomeBar.ChangeMinValue(value);
        StartCoroutine(Tools.Delay(UpdateBar, 0.1f));
    }

    private void OnDestroy() {
        if (_entityStorePoint == null) { return; }
        _entityStorePoint.CurrentValueChanged -= (float value) => { if (value < _entityStorePoint.CurrentValue) { _atomeBar.Remove(Mathf.Abs(value - _entityStorePoint.CurrentValue), false); } else { _atomeBar.Add(Mathf.Abs(value - _entityStorePoint.CurrentValue)); } };
        _entityStorePoint.MaxValueChanged -= (float value) => _atomeBar.ChangeMaxValue(value);
        _entityStorePoint.MinValueChanged -= (float value) => _atomeBar.ChangeMinValue(value);
    }

    private void UpdateBar() {
        _atomeBar.UpdateSlider();
    }
}
