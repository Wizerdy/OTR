using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class BossBall : MonoBehaviour {
    protected Rigidbody2D _rb;
    [SerializeField] protected Vector2 _startDirection;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _duration;
    [SerializeField] protected int _damages;
    [SerializeField] protected Vector2 _reminder;
    [SerializeField] protected Force _bounceForce;
    protected bool _mustDie = false;
    Timer _deathTimer;

    public BossBall ChangeSpeed(float speed) {
        _speed = speed;
        return this;
    }

    public BossBall ChangeDuration(float duration) {
        _duration = duration;
        return this;
    }

    public BossBall ChangeDamages(int damages) {
        _damages = damages;
        return this;
    }

    public BossBall ChangeStartDirection(Vector2 startDirection) {
        _startDirection = startDirection;
        return this;
    } 
    
    public BossBall ChangeForce(Force bounceForce) {
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

        if (collision.transform.CompareTag("Player")) {
            _bounceForce.Reset();
            EntityPhysics epPlayer = collision.gameObject.GetComponent<EntityAbilities>().Get<EntityPhysics>();
            EntityInvincibility eiPlayer = collision.gameObject.GetComponent<EntityAbilities>().Get<EntityInvincibility>();
            Vector2 direction = _rb.velocity.normalized;
            float dot = Vector2.Dot(direction, collision.transform.position - transform.position);
            if (dot > 0) {
                _bounceForce.Direction = Quaternion.Euler(0, 0, -90) * direction;
            } else {
                _bounceForce.Direction = Quaternion.Euler(0, 0, 90) * direction;
            }
            eiPlayer.ChangeCollisionLayer(_bounceForce.Duration);
            epPlayer.Add(new Force(_bounceForce), 10);
        }
    }

    void Die() {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
