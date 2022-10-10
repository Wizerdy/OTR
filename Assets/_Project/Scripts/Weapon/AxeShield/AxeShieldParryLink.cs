using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeShieldParryLink : MonoBehaviour
{
    [SerializeField] PlayerEntity _playerEntity;
    [SerializeField] ReflectiveCollider axeSHieldParry;

    void Start() {
        if (_playerEntity == null) { return; }
        _playerEntity.OnAim += _LinkAim;
    }

    private void OnDestroy() {
        if (_playerEntity != null) { return; }
        _playerEntity.OnAim -= _LinkAim;
    }

    private void _LinkAim(Vector2 direction) {
        axeSHieldParry.Aim = direction;
    }
}
