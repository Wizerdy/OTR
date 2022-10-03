using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EntityCollisionArea))]

public class EntityTryCatch : MonoBehaviour {
    [SerializeField] bool _canCatch;
    public bool CanCatch { get => _canCatch; set => _canCatch = value; }
    EntityCollisionArea _entityCollisionArea;
    float _radius;
    [SerializeField] CircleCollider2D _circleCollider2D;
    [SerializeField] EntityHolding _entityHolding;
    private void Reset() {
        _entityCollisionArea = GetComponent<EntityCollisionArea>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start() {
        if (_entityCollisionArea == null) {
            _entityCollisionArea = GetComponent<EntityCollisionArea>();
        }
        _radius = _circleCollider2D.radius;
    }

    public void TryCatch() {
        if (CanCatch && !_entityCollisionArea.IsEmpty()) {
            GameObject nearest = null;
            nearest = _entityCollisionArea.Nearest(pair => !pair.Key.gameObject.GetComponent<Weapon>().IsOnFloor);
            if (nearest != null) {
                _entityHolding.Pickup(nearest);
                CatchSuccess();
            } else { 
                CatchFailed();
            }
        } else {
            CatchFailed();
        }
    }

    void CatchSuccess() {
        Debug.Log("CatchSuccess");
    }

    void CatchFailed() {
        Debug.Log("CatchFailed");
    }
}