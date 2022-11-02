using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossChargeComeBack : BossCharge {
    [SerializeField] protected float _speedComeback;
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        yield return StartCoroutine(Charge(ea.transform.position, target.position, _delayBeforeCharge, _speed));
        yield return StartCoroutine(ChargeDestination(ea.transform, Vector3.zero, 0f, _speedComeback));
    }
}
