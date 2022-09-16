using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(EntityCollisionArea))]
[RequireComponent(typeof(ColliderDelegate))]

public class AtomeMagnet : MonoBehaviour {
    bool _isActive;
    public bool IsActive { get => _isActive; set => _isActive = value; }
    EntityCollisionArea _entityCollisionArea;
    ColliderDelegate _colliderDelegate;

    private void Start() {
        _entityCollisionArea = GetComponent<EntityCollisionArea>();
        _colliderDelegate = GetComponent<ColliderDelegate>();
    }

    void Update() {

    }

}
