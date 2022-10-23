using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public abstract class BossBall : MonoBehaviour {
    protected Rigidbody2D _rb;
    public Vector2 _startDirection;
    public float _speed;
    public float _duration;
    public Vector2 r;
    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = _startDirection.normalized * _speed;
        StartCoroutine(Tools.Delay(() => Destroy(gameObject),_duration));
    }

    private void Update() {
        r = _rb.velocity;
    }
    public abstract void Hit(Collision2D collision);


}
