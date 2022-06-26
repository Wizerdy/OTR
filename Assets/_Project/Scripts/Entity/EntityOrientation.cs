using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityOrientation : MonoBehaviour {
    [SerializeField] Transform _root;
    Vector2 _orientation = Vector2.up;

    public Vector2 Orientation => _orientation;

    public void LookAt(Vector2 direction) {
        direction.Normalize();
        _root.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        _orientation = direction;
    }
}
