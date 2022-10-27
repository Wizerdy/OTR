using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BossAttack {
    [Header("Charge :")]
    [SerializeField] float _speed;
    [SerializeField] float _duration;
    EntityMovement _entityMovement;
    public override void Activate(EntityAbilities ea, Transform target) {
        _entityMovement = ea.Get<EntityMovement>();
        _entityMovement.CreateMovement(_duration, _speed, target.position - ea.gameObject.transform.position);
    }

    public override void Activate(EntityAbilities ea, Transform[] targets) {
        
    }

    public void Hit(Collision2D collision) {
        if(collision.transform.tag == "Wall" && _entityMovement != null) {
            _entityMovement.StopMovement();
        }
    }
}
