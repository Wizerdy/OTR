using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class BarLinkerHealth : MonoBehaviour {
    [SerializeField] AtomeBar _atomeBar;
    [SerializeField] Health _health;

    private void Reset() {
        _atomeBar = GetComponent<AtomeBar>();
    }

    void Start() {
        if (_health == null) { return; }
        _health.OnHit += _OnHit;
        _health.OnHeal += _OnHeal;
        StartCoroutine(Tools.Delay(UpdateBar, 0.1f));
    }

    private void OnDestroy() {
        if (_health == null) { return; }
        _health.OnHit -= _OnHit;
        _health.OnHeal -= _OnHeal;
    }

    private void UpdateBar() {
        _atomeBar.ChangeMaxValue(_health.MaxHealth);
        _atomeBar.ChangeMinValue(0);
        _atomeBar.Add(_health.CurrentHealth);
        _atomeBar.UpdateSlider();
    }

    private void _OnHit(int value) {
        _atomeBar.Remove(value, false);
    }

    private void _OnHeal(int value) {
        _atomeBar.Add(value);
    }
}
