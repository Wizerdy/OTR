using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossCharge : BossAttack {
    [Header("Charge :")]
    [SerializeField] protected float _delayBeforeCharge;
    [SerializeField] protected float _speed;
    [SerializeField] protected Force _bounceForce;
    protected bool _hitWall = false;
    protected EntityPhysics _entityPhysics;
    protected EntityColliders _entityColliders;
    protected DamageHealth _damageHealth;
    protected Force _chargeForce;
    protected int _damagesMemory;
    protected Transform _transform;

    protected override IEnumerator AttackBegins(EntityAbilities ea, Transform target) {
        _hitWall = false;
        _hitWall = false;
        _entityPhysics = ea.Get<EntityPhysics>();
        _entityColliders = ea.Get<EntityColliders>();
        _transform = ea.transform;
        _damageHealth = _entityColliders.MainEvent.gameObject.GetComponent<DamageHealth>();
        _damagesMemory = _damageHealth.Damage;
        _damageHealth.SetDamage(_damages);
        _entityColliders.MainEvent.OnCollisionEnter += Hit;
        yield break;
    }
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        yield return StartCoroutine(Charge(target.position, _delayBeforeCharge, _speed));
    }

    protected override IEnumerator AttackEnds(EntityAbilities ea, Transform target) {
        _damageHealth.SetDamage(_damagesMemory);
        _entityColliders.MainEvent.OnCollisionEnter -= Hit;
        yield break;
    }

    protected IEnumerator Charge(Vector3 targetPosition, float delay, float speed) {
        Debug.DrawRay(transform.position, targetPosition - _transform.position, Color.blue, _delayBeforeCharge);
        yield return new WaitForSeconds(delay);
        _chargeForce = new Force(speed, targetPosition - _transform.position, 1, Force.ForceMode.INPUT, AnimationCurve.Linear(1f, 1f, 1f, 1f), 0.1f, AnimationCurve.Linear(0f, 0f, 0f, 0f), 0);
        _entityPhysics.Add(_chargeForce, 1);
        while (!_hitWall) {
            yield return null;
        }
        _entityPhysics.Remove(_chargeForce);
    }

    protected IEnumerator ChargeDestination(Vector3 destination, float delay, float speed) {
        yield return new WaitForSeconds(delay);
        _chargeForce = new Force(speed, destination - _transform.position, 1);
        _entityPhysics.Add(_chargeForce, 1);
        bool pass = false;
        float lastDist = Vector3.Distance(destination, _transform.position);
        while (!pass) {
            float currentDist = Vector3.Distance(destination, _transform.position);
            if (currentDist > lastDist) {
                pass = true;
            } else {
                lastDist = currentDist;
            }
            yield return null;
        }
        _entityPhysics.Remove(_chargeForce);
    }

    protected void Hit(Collision2D collision) {
        if (collision.transform.CompareTag("Wall")) {
            _hitWall = true;
        }
        if (collision.transform.CompareTag("Player")) {
            EntityAbilities eaPlayer = collision.gameObject.GetComponent<EntityAbilities>();
            EntityPhysics epPlayer = eaPlayer.Get<EntityPhysics>();
            EntityInvincibility eiPlayer = eaPlayer.Get<EntityInvincibility>();
            Vector2 direction = _entityPhysics.Velocity.normalized;
            float dot = Vector2.Dot(direction, collision.transform.position - _transform.position);
            if (dot > 0) {
                _bounceForce.Direction = Quaternion.Euler(0, 0, -90) * direction;
            } else {
                _bounceForce.Direction = Quaternion.Euler(0, 0, 90) * direction;
            }
            _bounceForce.Reset();
            eiPlayer.ChangeCollisionLayer(_bounceForce.Duration);
            epPlayer.Add(new Force(_bounceForce), 10);
        }
    }
}