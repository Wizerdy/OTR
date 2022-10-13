using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charge : BossAttack {
    [SerializeField] float _speed;
    public override void Activate(EntityAbilities ea, Transform target) {
        ea.Get<EntityMovement>().CreateMovement(10, _speed, target.position - ea.gameObject.transform.position);
    }
}
