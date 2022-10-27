using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class ChargeTeleport : BossAttack {
    [Header("ChargeTeleport :")]
    [SerializeField] float _speed;
    [SerializeField] float _duration;
    [SerializeField] float _delayCharge;
    [SerializeField] EntityMovement _entityMovement;
    public override void Activate(EntityAbilities ea, Transform target) {
        ea.transform.position = target.position + (Vector3.zero - target.position).normalized * 2;
        _entityMovement = ea.Get<EntityMovement>();
        Vector2 direction = target.position - ea.gameObject.transform.position;
        StartCoroutine(Tools.Delay(() => _entityMovement.CreateMovement(_duration, _speed, direction), _delayCharge));
    }

    public override void Activate(EntityAbilities ea, Transform[] targets) {

    }

    public void Hit(Collision2D collision) {
        if (collision.transform.tag == "Wall") {
            _entityMovement.StopMovement();
        }
    }


}
