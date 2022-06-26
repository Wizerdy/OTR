using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class InvulnerableFrame : MonoBehaviour {
    [SerializeField] Health _health;
    [SerializeField] float _time = 1f;

    private void Reset() {
        _health = GetComponent<Health>();
    }

    private void Start() {
        if (_health == null) { Destroy(this); }
        _health.OnHit += _Hitted;
    }

    private void OnDestroy() {
        _health.OnHit -= _Hitted;
    }

    private void _Hitted(int amount) {
        if (_time <= 0f) { return; }
        _health.CanTakeDamage = false;
        StartCoroutine(Tools.Delay(() => _health.CanTakeDamage = true, _time));
    }
}
