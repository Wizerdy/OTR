using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimSnapper : MonoBehaviour {
    [SerializeField] AimHelperReference _aimHelperReference;
    [SerializeField] Transform _target;
    [SerializeField] float _angle = 20f;

    private void Reset() {
        _target = transform;
    }

    private void OnEnable() {
        if (!_aimHelperReference.Valid()) { return; }
        _aimHelperReference.Instance.Add(_target, _angle);
    }

    private void Start() {
        if (!_aimHelperReference.Valid()) { return; }
        _aimHelperReference.Instance.Add(_target, _angle);
    }

    private void OnDisable() {
        if (!_aimHelperReference.Valid()) { return; }
        _aimHelperReference.Instance.Remove(_target);
    }
}
