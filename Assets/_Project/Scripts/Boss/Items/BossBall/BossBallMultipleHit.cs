using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class BossBallMultipleHit : BossBall {
    public override void Hit(Collision2D collision) {
        if(collision.collider.tag == "Wall")
        _rb.velocity = Vector2.Reflect(((Vector3)collision.GetContact(0).point - transform.position).normalized, collision.GetContact(0).normal) * _speed;
    }

}
