using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class AtomeBarHealth : MonoBehaviour {
    [SerializeField] AtomeBar _atomeBar;
    [SerializeField] EntityStorePoint _entityStorePoint;
    [SerializeField] Health _health;

    private void Reset() {
        _atomeBar = GetComponent<AtomeBar>();
    }

    void Start() {
        _health.OnHit += (int value) => _atomeBar.Remove(value, false);
        _health.OnHeal += (int value) => _atomeBar.Add(value);
        if (_entityStorePoint != null)
            _entityStorePoint.TooMuchPointLoseDifference += (float value) => _atomeBar.Remove(value, true);
        StartCoroutine(Tools.Delay(UpdateBar, 0.1f));
    }

    private void OnDestroy() {
        _health.OnHit += (int value) => _atomeBar.Remove(value, false);
        _health.OnHeal += (int value) => _atomeBar.Add(value);
        if (_entityStorePoint != null)
            _entityStorePoint.TooMuchPointLoseDifference += (float value) => _atomeBar.Remove(value, true);
    }

    private void UpdateBar() {
        _atomeBar.Add(_health.CurrentHealth);
        _atomeBar.ChangeMaxValue(_health.MaxHealth);
        _atomeBar.ChangeMinValue(0);
        _atomeBar.UpdateSlider();
    }
}
