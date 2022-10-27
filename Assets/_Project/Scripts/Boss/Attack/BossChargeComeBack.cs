using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossChargeComeBack : BossCharge {
    protected override IEnumerator Attack(EntityAbilities ea, Transform target) {
        _entityMovement = ea.Get<EntityMovement>();
        yield return StartCoroutine(Charge(ea.transform.position, target.position));
        yield return StartCoroutine(ChargeDestination(ea.transform.position, Vector3.zero));
    }

    protected IEnumerator ChargeDestination(Vector3 ownPosition, Vector3 destination) {
        _entityMovement.CreateMovement(_duration, _speed, destination - ownPosition);
        yield return null;
    }
}
