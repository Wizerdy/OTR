using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkReflectableToAim : MonoBehaviour {
    [SerializeField] PlayerEntity _playerEntity;
    [SerializeField] ReflectiveCollider _reflectiveCollider;

    void Start() {
        if (_playerEntity == null) { return; }
        _playerEntity.OnAim += _LinkAim;
    }

    private void OnDestroy() {
        if (_playerEntity != null) { return; }
        _playerEntity.OnAim -= _LinkAim;
    }

    private void _LinkAim(Vector2 direction) {
        _reflectiveCollider.Aim = direction;
    }
}
