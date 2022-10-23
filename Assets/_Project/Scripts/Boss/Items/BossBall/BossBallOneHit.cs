using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBallOneHit : BossBall {
    public override void Hit(Collision2D collision) {
        if(collision.collider.tag == "Player")
        Destroy(gameObject);

        if (collision.collider.tag == "Wall")
            _rb.velocity = Vector2.Reflect(r.normalized, collision.GetContact(0).normal) * _speed;

        Debug.Log(_rb.velocity.normalized);
        Debug.Log(collision.GetContact(0).normal);
        Debug.Log(Vector2.Reflect(_rb.velocity.normalized, collision.GetContact(0).normal) * _speed);
    }
}
