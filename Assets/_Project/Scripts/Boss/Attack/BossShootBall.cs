using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossShootBall : BossAttack {
    [Header("Pas Touche :")]
    [SerializeField] BossBall _bossBallprefab;
    [Header("Ball :")]
    [SerializeField] float _ballSpeed;
    [SerializeField] float _ballDuration;
    protected override IEnumerator Attack(EntityAbilities ea, Transform target) {
        LaunchBall(ea.transform.position, target.position);
        yield return StartCoroutine(Disactivate());
    }


    protected void LaunchBall(Vector3 ourPosition, Vector3 targetPosition) {
        BossBall newBall = Instantiate(_bossBallprefab);
        newBall.transform.position = ourPosition + (targetPosition - ourPosition).normalized;
        newBall._startDirection = (targetPosition - ourPosition).normalized;
        newBall._speed = _ballSpeed;
        newBall._duration = _ballDuration;
    }
}
