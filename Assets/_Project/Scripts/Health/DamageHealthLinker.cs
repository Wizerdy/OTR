using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageHealthLinker : MonoBehaviour {
    [SerializeField] DamageHealth _damageHealth;
    [SerializeField] ColliderDelegate[] _colliderDelegate;

    private void Reset() {
        _damageHealth = GetComponent<DamageHealth>();
    }

    void Awake() {
        for (int i = 0; i < _colliderDelegate.Length; i++) {
            if (_colliderDelegate[i] != null) {
                _colliderDelegate[i].OnCollisionEnter += _damageHealth.Collision;
                _colliderDelegate[i].OnTriggerEnter += _damageHealth.Trigger;
            }
        }
    }

    private void OnDestroy() {
        for (int i = 0; i < _colliderDelegate.Length; i++) {
            if (_colliderDelegate[i] != null) {
                _colliderDelegate[i].OnCollisionEnter -= _damageHealth.Collision;
                _colliderDelegate[i].OnTriggerEnter -= _damageHealth.Trigger;
            }
        }
    }
}
