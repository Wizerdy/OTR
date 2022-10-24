using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigBossBall : BossBall {
    [SerializeField] float _angle;
    [SerializeField] BossBall _childrenPrefab;
    public override void Hit(Collision2D collision) {
        if (collision.collider.tag == "Wall")
            Multiply(Vector2.Reflect(r.normalized, collision.GetContact(0).normal));
    }

    public void Multiply(Vector2 direction) {
        BossBall newBossBall1 = Instantiate(_childrenPrefab, transform.position, Quaternion.identity);
        BossBall newBossBall2 = Instantiate(_childrenPrefab, transform.position, Quaternion.identity);
        BossBall newBossBall3 = Instantiate(_childrenPrefab, transform.position, Quaternion.identity);
        newBossBall1._startDirection = direction;
        newBossBall2._startDirection = Quaternion.Euler(new Vector3(0, 0, _angle)) * (Vector3)direction;
        newBossBall3._startDirection = Quaternion.Euler(new Vector3(0, 0, -_angle)) * (Vector3)direction;
        newBossBall1._speed = _speed;
        newBossBall2._speed = _speed;
        newBossBall3._speed = _speed;
        newBossBall1._duration = _duration;
        newBossBall2._duration = _duration;
        newBossBall3._duration = _duration;
        Destroy(gameObject);
    }
}
