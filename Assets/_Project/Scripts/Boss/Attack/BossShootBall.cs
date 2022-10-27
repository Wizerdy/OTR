using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosShootBall : BossAttack {
    [SerializeField] BossBall _bossBallprefab;
    [SerializeField] float _speed;
    [SerializeField] float _duration;
    protected override IEnumerator Attack(EntityAbilities ea, Transform target) {
        LaunchBall(ea.transform.position, target.position);
        yield return EndAttack();
    }

    protected void LaunchBall(Vector3 ourPosition, Vector3 targetPosition) {
        BossBall newBall = Instantiate(_bossBallprefab);
        newBall.transform.position = ourPosition + (targetPosition - ourPosition).normalized;
        newBall._startDirection = (targetPosition - ourPosition).normalized;
        newBall._speed = _speed;
        newBall._duration = _duration;
    }
}
