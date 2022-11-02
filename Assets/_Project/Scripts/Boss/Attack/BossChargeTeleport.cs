using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class BossChargeTeleport : BossCharge {
    [SerializeField] float _delayBeforeTeleport;
    [SerializeField] float _distFromPlayer;
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        yield return new WaitForSeconds(_delayBeforeTeleport);
        ea.transform.position = target.position + (Vector3.zero - target.position).normalized * _distFromPlayer;
        yield return StartCoroutine(Charge(ea.transform.position, target.position, _delayBeforeCharge, _speed));
    }
}
