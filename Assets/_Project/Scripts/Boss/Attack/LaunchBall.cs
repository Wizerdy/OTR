using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchBall : BossAttack {
    [SerializeField] BossBall _bossBallprefab;
    [SerializeField] float _speed;
    [SerializeField] float _duration;
    public override void Activate(EntityAbilities ea, Transform target) {
        BossBall newBall = Instantiate(_bossBallprefab);
        newBall.transform.position = ea.transform.position;
        newBall._startDirection = (target.position - ea.transform.position).normalized;
        newBall._speed = _speed;
        newBall._duration = _duration;
    }

    public override void Activate(EntityAbilities ea, Transform[] targets) {

    }
}
