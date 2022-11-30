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
        if (collision.transform.CompareTag("Player")) {
            EntityPhysics epPlayer = collision.gameObject.GetComponent<EntityAbilities>().Get<EntityPhysics>();
            EntityInvincibility eiPlayer = collision.gameObject.GetComponent<EntityAbilities>().Get<EntityInvincibility>();
            Vector2 direction = _rb.velocity.normalized;
            float dot = Vector2.Dot(direction, collision.transform.position - transform.position);
            if (dot > 0) {
                _bounceForce.Direction = Quaternion.Euler(0, 0, -90) * direction;
            } else {
                _bounceForce.Direction = Quaternion.Euler(0, 0, 90) * direction;
            }
            _bounceForce.Reset();
            eiPlayer.ChangeCollisionLayer(_bounceForce.Duration);
            epPlayer.Add(new Force(_bounceForce), 10);
            if (_destroyOnPlayerHit) {
                Die();
            }
        }
    }

    public void Multiply(Vector2 direction) {
        BossBall[] newBossBall = new BossBall[3];
        for (int i = 0; i < newBossBall.Length; i++) {
            newBossBall[i] = Instantiate(_childrenPrefab, transform.position, Quaternion.identity)
                .ChangeDamages(_damages)
                .ChangeDuration(_duration)
                .ChangeSpeed(_speed)
                .ChangeStartDirection(Quaternion.Euler(new Vector3(0, 0, -_angle + _angle * i)) * (Vector3)direction)
                .ChangeForce(_bounceForce);
        }
        Destroy(gameObject);
    }
}
