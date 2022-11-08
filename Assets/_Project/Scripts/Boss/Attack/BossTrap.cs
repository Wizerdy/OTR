using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTrap : BossAttack {
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        yield break;
    }
}
