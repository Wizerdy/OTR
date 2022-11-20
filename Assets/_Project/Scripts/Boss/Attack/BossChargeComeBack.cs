using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossChargeComeBack : BossCharge {
    [SerializeField] protected float _speedComeback;
    [SerializeField] protected float _delayBeforeComeBack;
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        yield return StartCoroutine(Charge(target.position, _delayBeforeCharge, _speed));
        yield return StartCoroutine(ChargeDestination(_center.position, _delayBeforeComeBack, _speedComeback));
    }
}
