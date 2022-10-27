using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class BossChargeTeleport : BossCharge {
    
    protected override IEnumerator Attack(EntityAbilities ea, Transform target) {
        ea.transform.position = target.position + (Vector3.zero - target.position).normalized * 2;
        _entityMovement = ea.Get<EntityMovement>();
        Vector2 direction = target.position - ea.gameObject.transform.position;
        yield return StartCoroutine(Tools.Delay(() => _entityMovement.CreateMovement(_duration, _speed, direction), _delayCharge));
    }
}
