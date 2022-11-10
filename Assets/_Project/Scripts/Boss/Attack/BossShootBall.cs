using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShootBall : BossAttack {
    [Header("Pas Touche :")]
    [SerializeField] BossBall _bossBallprefab;
    [Header("Ball :")]
    [SerializeField] float _ballSpeed;
    [SerializeField] float _ballDuration;
    [SerializeField] Force _bounceForce;
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        _ea.Get<EntityBoss>().PlayAnimationTrigger("Projectiling");
        LaunchBall(ea.transform.position, target.position);
        yield break;
    }


    protected void LaunchBall(Vector3 ourPosition, Vector3 targetPosition) {
        BossBall newBall = Instantiate(_bossBallprefab).ChangeDamages(_damages).ChangeDuration(_ballDuration).ChangeSpeed(_ballSpeed).ChangeStartDirection((targetPosition - ourPosition).normalized).ChangeForce(_bounceForce);
        newBall.transform.position = ourPosition + (targetPosition - ourPosition).normalized;
    }
}
