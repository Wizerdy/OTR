using System.Collections;
using UnityEngine;

public class BossChargeComeBack : BossCharge {
    [SerializeField] protected float _speedComeback;
    [SerializeField] protected float _delayBeforeComeBack;
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        yield return StartCoroutine(Charge(target.position, _speed));
        yield return StartCoroutine(ChargeDestination(_center.position, _speedComeback));
    }
}
