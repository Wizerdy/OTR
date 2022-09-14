using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sloot;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class ChakramShockwave : MonoBehaviour {
    [SerializeField] public Vector2 _direction;
    [SerializeField] float _time;
    [SerializeField] float _baseSpeed;
    [SerializeField] float _stackMultiplicator;
    [SerializeField] public int _stack;
    [SerializeField] public int _maxstack;
    Rigidbody2D rb;
    SpriteRenderer sr;
    Timer _duration;


    private void Start() {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
        Power();
        sr.color = new Color(1f / _maxstack * _stack, 1f / _maxstack * _stack, 1f / _maxstack * _stack);
        _duration = new Timer(this, _time);
        _duration.OnActivate += Die;
        _duration.Start();
    }

    void Power() {
        int super = 1;
        if(_stack > _maxstack) {
            _stack = _maxstack;
            super = 4;
            transform.localScale = new Vector3(2, 2, 2);
        }
        rb.velocity = _direction.normalized * _baseSpeed * _stackMultiplicator * _stack * super;
    }

    void Die() {
        Destroy(gameObject);
    }
}
