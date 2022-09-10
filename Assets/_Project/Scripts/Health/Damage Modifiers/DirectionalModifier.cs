using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class DirectionalModifier : DamageModifier {
    [SerializeField] Transform _root;
    [SerializeField] Vector2 _direction = Vector2.up;
    [SerializeField] float _angle = 10f;

    public Vector2 Direction { get => _direction; set => _direction = value; }

    private void Reset() {
        _root = transform;
    }

    protected override bool Usable(int value, GameObject source) {
        if (_direction == Vector2.zero) { return false; }
        if (_root == null) { _root = transform; }
        if (source == null) { return false; }

        Vector2 direction = source.transform.position - _root.position;
        if (Vector2.Angle(direction, _direction) <= _angle / 2f) {
            return true;
        }
        return false;
    }

    private void OnDrawGizmosSelected() {
        if (_direction == Vector2.zero) { return; }
        if (_root == null) { return; }

        Gizmos.color = Color.red;
        Gizmos.DrawLine(_root.position.To2D(), _root.position.To2D() + _direction);
        Gizmos.color = Color.blue;
        Quaternion angle = Quaternion.Euler(0f, 0f, _angle / 2f);
        Gizmos.DrawLine(_root.position.To2D(), _root.position + angle * _direction);
        Gizmos.DrawLine(_root.position.To2D(), _root.position + Quaternion.Inverse(angle) * _direction);
    }
}
