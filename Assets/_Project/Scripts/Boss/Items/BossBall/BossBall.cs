using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using TMPro;
using System;

public class BossBall : MonoBehaviour, IReflectable {
    protected Rigidbody2D _rb;
    [SerializeField] protected Vector2 _startDirection;
    [SerializeField] protected float _speed;
    [SerializeField] protected float _duration;
    [SerializeField] protected int _damages;
    [SerializeField] protected Vector2 _reminder;
    [SerializeField] protected Vector2 _lastDir;
    [SerializeField] protected Force _bounceForce;
    [SerializeField] protected bool _destroyOnPlayerHit = false;
    [SerializeField] protected int _damageReflect = 30;
    protected bool _mustDie = false;
    protected Timer _deathTimer;
    [SerializeField] protected BossBallParried parriedBossBallPrefab;
    [SerializeField] protected DamageHealth _damageHealth;

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


    protected void Start() {
        _rb = GetComponent<Rigidbody2D>();
        _rb.velocity = _startDirection.normalized * _speed;
        _lastDir = _rb.velocity;
        _damageHealth.Damage = _damages;
        _damageHealth.OnDamage += _OnDamage;
        _deathTimer = new Timer(this, _duration, false);
        _deathTimer.OnActivate += () => _mustDie = true;
        _deathTimer.Start(_duration);

    }

    private void FixedUpdate() {
        if(_rb.velocity.magnitude < 0.05f) {
            _rb.velocity = _lastDir;
        } 
        _reminder = _rb.velocity;
    }

    public virtual void Hit(Collision2D collision) {
        if (collision.collider.tag == "Wall") {
            _lastDir = _rb.velocity;
            if (_mustDie) {
                Die();
            }
        }

        if (collision.transform.CompareTag("Player")) {
            //EntityAbilities ea = collision.gameObject.GetComponent<EntityAbilities>();
            //EntityPhysics epPlayer = ea.Get<EntityPhysics>();
            //EntityInvincibility eiPlayer = ea.Get<EntityInvincibility>();

            //Vector2 direction = _reminder;
            //float dot = Vector2.Dot(direction, collision.transform.position - transform.position);
            //if (dot > 0) {
            //    _bounceForce.Direction = Quaternion.Euler(0, 0, -90) * direction;
            //} else {
            //    _bounceForce.Direction = Quaternion.Euler(0, 0, 90) * direction;
            //}
            //_bounceForce.Reset();
            //eiPlayer.ChangeCollisionLayer(_bounceForce.Duration);
            //epPlayer.Add(new Force(_bounceForce), 10);
            if (_destroyOnPlayerHit) {
                Die();
            }
        }
    }

    protected void _OnDamage(IHealth target, int damages) {
        if (damages <= 0) { return; }
        EntityAbilities abilities = target.GameObject.GetComponentInRoot<EntityAbilities>();
        if (abilities == null) { return; }
        EntityPhysics entityPhysics = abilities.Get<EntityPhysics>();
        if (entityPhysics == null) { return; }

        Vector2 direction = _reminder;
        float dot = Vector2.Dot(direction, abilities.transform.position - transform.position);
        if (dot > 0) {
            _bounceForce.Direction = Quaternion.Euler(0, 0, -90) * direction;
        } else {
            _bounceForce.Direction = Quaternion.Euler(0, 0, 90) * direction;
        }
        _bounceForce.Reset();
        entityPhysics.Add(new Force(_bounceForce), 10);

        abilities.Get<EntityInvincibility>()?.ChangeCollisionLayer(_bounceForce.Duration);
    }

    protected void Die() {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    public void Reflect(ContactPoint2D collision) {
        // non reflectable
    }

    public void Launch(float force, Vector2 direction) {
        //_rb.velocity = force * direction;
        //newBall.transform.position = transform + (targetPosition - ourPosition).normalized;
        BossBallParried newBall = Instantiate(parriedBossBallPrefab).ChangeDamages(_damageReflect).ChangeDuration(_duration).ChangeSpeed(force).ChangeForce(_bounceForce);
        newBall.transform.position = transform.position;
        if (direction != Vector2.zero) {
            newBall.ChangeStartDirection(direction);
        } else {
            Vector2 newDir = _reminder * -1;
            newBall.ChangeStartDirection(newDir);
        }

        Die();
    }
}
