using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class DummyEntity : MonoBehaviour {
    [SerializeField] Health _health;
    [SerializeField] Animator _animator;
    [SerializeField] GameObject _colliders;
    [SerializeField] float _reviveTime = 3f;
    [SerializeField] PowerUp _powerUp;

    private void Start() {
        if (_health != null) {
            _health.OnDeath += _OnDeath;
        }
        _powerUp?.Clone().Enable();
    }

    private void _OnDeath() {
        _colliders?.SetActive(false);
        _animator.SetBool("Dead", true);
        StartCoroutine(Tools.Delay(Revive, _reviveTime));
    }

    private void Revive() {
        _colliders?.SetActive(true);
        _animator.SetBool("Dead", false);
        _health.TakeHeal(_health.MaxHealth);
    }
}
