using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class Bolt : MonoBehaviour {
    [SerializeField] DamageHealth _damageHealth;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] BetterEvent<Collision2D> _onCollide;
    [SerializeField] BetterEvent<IHealth, int> _onHit;

    float _speed = 5f;
    int _damage = 5;
    Vector2 _direction = Vector2.up;

    public event UnityAction<Collision2D> OnCollide { add => _onCollide += value; remove => _onCollide -= value; }
    public event UnityAction<IHealth, int> OnHit { add => _onHit += value; remove => _onHit -= value; }

    private void Reset() {
        _damageHealth = GetComponentInChildren<DamageHealth>();
        _rb = GetComponentInChildren<Rigidbody2D>();
    }

    private void Start() {
        if (_damageHealth != null) {
            _damageHealth.OnCollide += _InvokeOnCollider;
            _damageHealth.OnDamage += _InvokeOnHit;
        }
    }

    public Bolt SetDamage(int damage) {
        _damage = damage;
        _damageHealth?.SetDamage(_damage);
        return this;
    }

    public Bolt SetSpeed(float speed) {
        _speed = speed;
        _rb.velocity = speed * _direction;
        return this;
    }

    public Bolt SetDirection(Vector2 direction) {
        _direction = direction;
        _rb.velocity = _speed * direction;
        return this;
    }

    private void _InvokeOnCollider(Collision2D collider) {
        _onCollide.Invoke(collider);
    }

    private void _InvokeOnHit(IHealth health, int damage) {
        _onHit.Invoke(health, damage);
    }
}
