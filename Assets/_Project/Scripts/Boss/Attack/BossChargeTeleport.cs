using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class BossChargeTeleport : BossCharge {
    [SerializeField] float _delayBeforeTeleport;
    [SerializeField] float _distFromPlayer;
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        _entityBoss.SetAnimationTrigger("StartTp");
        while (!_entityBoss.GetAnimationBool("CanTp1")) {
            yield return null;
        }
        ea.transform.position = target.position + (Vector3.zero - target.position).normalized * _distFromPlayer;
        _entityBoss.SetAnimationTrigger("EndTp");
        while (!_entityBoss.GetAnimationBool("CanTp2")) {
            yield return null;
        }
        _entityBoss.SetAnimationBool("CanTp1", false);
        _entityBoss.SetAnimationBool("CanTp2", false);
        yield return Charge(target.position, _speed);
    }

    IEnumerator cef() {
        yield return null;
    }
}
