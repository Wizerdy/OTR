using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBoy : MonoBehaviour, IReflectable {
    [SerializeField] Rigidbody2D _rb;

    Vector2 _lastVelocity;

    private void Update() {
        _lastVelocity = _rb?.velocity ?? Vector2.zero;
    }

    public void Launch(float force, Vector2 direction) {
        if (direction == Vector2.zero) { return; }
        direction.Normalize();
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        _rb.velocity = direction * force;
    }

    public void Reflect(ContactPoint2D collision) {
        if (_lastVelocity.sqrMagnitude <= 0.5f) { return; }
        Vector2 reflection = Vector2.Reflect(_lastVelocity, collision.normal);
        _rb.velocity = reflection;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _rb.velocity);
    }
}
