using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationLinker : MonoBehaviour {
    [SerializeField] EntityOrientation _entityOrientation;

    private void OnEnable() {
        if (_entityOrientation != null) {
            _entityOrientation.OnOrientation += Orientate;
        }
    }

    private void OnDisable() {
        if (_entityOrientation != null) {
            _entityOrientation.OnOrientation -= Orientate;
        }
    }

    private void Orientate(Vector2 direction) {
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
    }
}
