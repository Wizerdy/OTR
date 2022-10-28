using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class BossChargeTeleport : BossCharge {
    [SerializeField] float _delayBeforeTeleport;
    [SerializeField] float _distFromPlayer;
    protected override IEnumerator Attack(EntityAbilities ea, Transform target) {
        _hitWall = false;
        _entityMovement = ea.Get<EntityMovement>();
        _entityColliders = ea.Get<EntityColliders>();
        yield return new WaitForSeconds(_delayBeforeTeleport);
        ea.transform.position = target.position + (Vector3.zero - target.position).normalized * _distFromPlayer;
        _entityColliders.Main.OnTriggerEnter += Hit;
        yield return StartCoroutine(Charge(ea.transform.position, target.position, _delayBeforeCharge, _speed));
        _entityMovement.StopMovement();
        _entityColliders.Main.OnTriggerEnter -= Hit;
        yield return StartCoroutine(Disactivate());
    }
}
