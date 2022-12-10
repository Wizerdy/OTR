using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class UIBarHealthLinker : MonoBehaviour {
    [SerializeField] HealthReference _health;
    [SerializeField] UIBar _bar;

    private void Reset() {
        _bar = GetComponent<UIBar>();
    }

    void Start() {
        if (!_health.IsValid()) { return; }
        Init();
        _health.Instance.OnHeal += _OnChange;
        _health.Instance.OnHit += _OnChange;
        _health.Instance.OnMaxHealthChange += _OnChange;
    }

    private void _OnChange(int _) {
        if (!_health.IsValid()) { return; }
        _bar.Set(_health.Instance.Percentage);
    }

    private void Init() {
        if (!_health.IsValid()) { return; }
        _bar.SplitSecondSet(_health.Instance.Percentage);
    }
}
