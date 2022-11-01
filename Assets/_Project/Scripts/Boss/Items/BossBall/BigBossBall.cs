using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BigBossBall : BossBall {
    [SerializeField] float _angle;
    [SerializeField] BossBall _childrenPrefab;
    public override void Hit(Collision2D collision) {
        if (collision.collider.tag == "Wall")
            Multiply(Vector2.Reflect(_reminder.normalized, collision.GetContact(0).normal));
    }

    public void Multiply(Vector2 direction) {
        BossBall[] newBossBall = new BossBall[3];
        for (int i = 0; i < newBossBall.Length; i++) {
            newBossBall[i] = Instantiate(_childrenPrefab, transform.position, Quaternion.identity)
                .ChangeDamages(_damages)
                .ChangeDuration(_duration)
                .ChangeSpeed(_speed)
                .ChangeStartDirection(Quaternion.Euler(new Vector3(0, 0, -_angle + _angle * i)) * (Vector3)direction);
        }
        Destroy(gameObject);
    }
}
