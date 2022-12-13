using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceMe : MonoBehaviour {
    [SerializeField] EntityPhysics _target;
    [SerializeField] ForceData _force;

    void Start() {
        _target.Add(new Force(_force, Vector2.right, 1f, Force.ForceMode.TIMED), 100);
    }
}
