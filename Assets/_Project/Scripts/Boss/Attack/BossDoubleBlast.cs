using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoubleBlast : BossBlast {

    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        Vector3 positiontoShoot = target.position;
        yield return base.AttackMiddle(ea, target);
        yield return new WaitForSeconds(_chargeDuration);
        Blast(ea.transform.position, ea.transform.position + (ea.transform.position - positiontoShoot));
        yield return new WaitForSeconds(_blastDuration);
    }
}
