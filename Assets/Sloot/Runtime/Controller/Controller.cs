using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {
    [SerializeField] protected Vector2 _direction;
    [SerializeField] protected bool _space;

    public Vector2 Direction { get { return _direction; } }
    public bool Space { get { return _space; } }

}
