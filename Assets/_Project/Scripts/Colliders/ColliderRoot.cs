using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderRoot : MonoBehaviour {
    [SerializeField] GameObject _root;

    public GameObject Root => _root;
}
