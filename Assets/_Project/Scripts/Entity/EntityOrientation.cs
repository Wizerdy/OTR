using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class EntityOrientation : MonoBehaviour, IEntityAbility {
    [SerializeField] Transform _root;

    [HideInInspector, SerializeField] BetterEvent<Vector2> _onOrientation = new BetterEvent<Vector2>();

    Vector2 _orientation = Vector2.up;

    public Vector2 Orientation => _orientation;

    public event UnityAction<Vector2> OnOrientation { add => _onOrientation += value; remove => _onOrientation.RemoveListener(value); }

    public void LookAt(Vector2 direction) {
        if (_root == null) { return; }
        if (direction != _orientation) { _onOrientation.Invoke(direction); }
        if (direction == Vector2.zero) { return; }
        direction.Normalize();
        _root.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        _orientation = direction;
    }
}
