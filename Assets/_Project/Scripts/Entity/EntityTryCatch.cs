using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;



public class EntityTryCatch : MonoBehaviour, IEntityAbility {
    [SerializeField] bool _canCatch;
    public bool CanCatch { get => _canCatch; set => _canCatch = value; }
    float _radius;
    [SerializeField] EntityCollisionArea _entityCollisionArea;
    [SerializeField] CircleCollider2D _circleCollider2D;
    [SerializeField] EntityHolding _entityHolding;
    private void Start() {
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