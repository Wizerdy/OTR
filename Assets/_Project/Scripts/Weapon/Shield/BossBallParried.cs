using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBallParried : MonoBehaviour
{
    protected Rigidbody2D _rb;
    [SerializeField] protected Vector2 _startDirection;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _duration;
    [SerializeField] protected int _damages;
    [SerializeField] protected Vector2 _reminder;
    [SerializeField] protected Force _bounceForce;
    protected bool _mustDie = false;
    Timer _deathTimer;

    public BossBallParried ChangeSpeed(float speed) {
        _speed = speed;
        return this;
    }

    public BossBallParried ChangeDuration(float duration) {
        _duration = duration;
        return this;
    }

    public BossBallParried ChangeDamages(int damages) {
        _damages = damages;
        return this;
    }

    public BossBallParried ChangeStartDirection(Vector2 startDirection) {
        _startDirection = startDirection;
        return this;
    }

    public BossBallParried ChangeForce(Force bounceForce) {
        _bounceForce = bounceForce;
        return this;
    }


    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = _startDirection.normalized * _speed;
        GetComponent<DamageHealth>().Damage = _damages;
        _deathTimer = new Timer(this, _duration, false);
        _deathTimer.OnActivate += () => _mustDie = true;
        _deathTimer.Start(_duration);
    }

    private void Update() {
        _reminder = _rb.velocity;
    }
    public virtual void Hit(Collision2D collision) {
        if (collision.collider.tag == "Wall") {
            if (_mustDie) {
                Die();
            } else {
                _rb.velocity = Vector2.Reflect(_reminder.normalized, collision.GetContact(0).normal) * _speed;
            }
        }
    }

    void Die() {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
