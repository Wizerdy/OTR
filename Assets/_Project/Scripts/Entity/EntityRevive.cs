using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRevive : MonoBehaviour {
    [SerializeField] Health _health;
    [SerializeField] Collider2D _collider;

    [Header("System")]
    [SerializeField] int _pressCount = 10;
    [SerializeField] float _healthPercentage = 0.5f;

    int _currentPress;

    void Start() {
        _health.OnDeath += _Death;
    }

    void _Death() {
        _collider.isTrigger = true;
    }

    public void HeartMassage() {
        ++_currentPress;
        if (_currentPress >= _pressCount) {
            _currentPress = 0;
            Revive();
        }
    }

    public void Revive() {
        _collider.isTrigger = false;
        _health.TakeHeal(_healthPercentage);
    }
}
