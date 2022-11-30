using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackReferenceGetter : BossAttack {
    [SerializeField] TransformReference _centerRef;
    [SerializeField] TransformReference _topLeftRef;
    [SerializeField] TransformReference _botRightRef;
    [SerializeField] TransformReference _spawnBallRightRef;
    [SerializeField] TransformReference _spawnBallLeftRef;

    private void Start() {
        _center = _centerRef.Instance;
        _topLeft = _topLeftRef.Instance;
        _botRight = _botRightRef.Instance;
        _spawnBallRight = _spawnBallRightRef.Instance;
        _spawnBallLeft = _spawnBallLeftRef.Instance;
    }
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        yield break;
    }
}
