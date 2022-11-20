using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossShootBall : BossAttack {
    [Header("Pas Touche :")]
    [SerializeField] BossBall _bossBallprefab;
    [Header("Ball :")]
    [SerializeField] float _ballSpeed;
    [SerializeField] float _ballDuration;
    [SerializeField] Force _bounceForce;
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        _entityBoss.SetAnimationTrigger("Projectiling");
        Vector3 direction = (target.position - ea.transform.position).normalized;
        Transform spawn;
        if (Vector3.Dot(Vector3.right, direction) < 0) {
            _entityBoss.FlipRight(false);
            spawn = _spawnBallLeft;
        } else {
            _entityBoss.FlipRight(true);
            spawn = _spawnBallRight;
        }
        while (!_entityBoss.GetAnimationBool("CanShoot")) {
            yield return null;
        }
        LaunchBall(spawn.position, target.position);
        _entityBoss.SetAnimationBool("CanShoot", false);
    }


    protected void LaunchBall(Vector3 spawnPosition, Vector3 targetPosition) {
        BossBall newBall = Instantiate(_bossBallprefab).ChangeDamages(_damages).ChangeDuration(_ballDuration).ChangeSpeed(_ballSpeed).ChangeStartDirection((targetPosition - spawnPosition).normalized).ChangeForce(_bounceForce);
        newBall.transform.position = spawnPosition;
    }
}
