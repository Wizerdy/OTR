using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BossAttack {
    [SerializeField] float _speed;
    [SerializeField] float _duration;
    public override void Activate(EntityAbilities ea, Transform target) {
        ea.Get<EntityMovement>().CreateMovement(_duration, _speed, target.position - ea.gameObject.transform.position);
    }

    public override void Activate(EntityAbilities ea, Transform[] targets) {
        
    }
}
