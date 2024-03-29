using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class BarLinkerHealth : MonoBehaviour {
    [SerializeField] AtomeBar _atomeBar;
    [SerializeField] HealthReference _health;
    [SerializeField] private BossBar bossBar;

    private void Reset() {
        _atomeBar = GetComponent<AtomeBar>();
    }

    void Start() {
        if (!_health.IsValid()) { return; }
        _health.Instance.OnHit += _OnHit;
        _health.Instance.OnHeal += _OnHeal;
        StartCoroutine(Tools.Delay(UpdateBar, 0.1f));
    }

    private void OnDestroy() {
        if (!_health.IsValid()) { return; }
        _health.Instance.OnHit -= _OnHit;
        _health.Instance.OnHeal -= _OnHeal;
    }

    public void UpdateBar() {
        _atomeBar.ChangeMaxValue(_health.Instance.MaxHealth);
        _atomeBar.ChangeMinValue(0);
        _atomeBar.Add(_health.Instance.CurrentHealth);
        _atomeBar.UpdateSlider();
        bossBar?.ReduceNumberOfBar(1);
    }

    private void _OnHit(int value) {
        _atomeBar.Remove(value, false);
    }

    private void _OnHeal(int value) {
        _atomeBar.Add(value);
    }
}
