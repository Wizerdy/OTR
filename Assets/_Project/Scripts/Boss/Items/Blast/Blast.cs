using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blast : MonoBehaviour {
    Vector2[] _points;
    PolygonCollider2D _polygonCollider;
    Rigidbody2D _rb;
    ColliderDelegate _colliderDelegate;

    private void Start() {
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _rb = GetComponent<Rigidbody2D>();
        _colliderDelegate = GetComponent<ColliderDelegate>();
        _polygonCollider.points = _points;
    }
    public void ChangePoints(Vector2[] points) {
        _points = points;
    }

    public void Hit(Collision2D collision) {
        Debug.Log(collision.gameObject.name);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log(collision.gameObject.name);
    }
}
