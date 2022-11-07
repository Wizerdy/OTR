using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossCage : BossAttack {
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform transform) {
        yield break;
    }
}
