using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossCharge : BossAttack {
    [Header("Charge :")]
    [SerializeField] protected float _delayBeforeCharge;
    [SerializeField] protected float _speed;
    [SerializeField] protected Force _bounceForce;
    [SerializeField] protected Force _bounceWallForce;
    protected bool _hitWall = false;
    protected DamageHealth _damageHealth;
    protected Force _chargeForce;
    protected int _damagesMemory;
    protected Vector3 _bounceWallDirection;

    protected override IEnumerator AttackBegins(EntityAbilities ea, Transform target) {
        _hitWall = false;
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
        _entityBoss.SetAnimationBool("HitWall", false);
        Vector3 direction = targetPosition - _transform.position;
        if (Vector3.Dot(Vector3.right, direction) < 0) {
            _entityBoss.FlipRight(false);
        } else {
            _entityBoss.FlipRight(true);
        }
        Debug.DrawRay(transform.position, direction, Color.blue, _delayBeforeCharge);
        yield return new WaitForSeconds(delay);
        _entityBoss.SetAnimationBool("Charging", true);
        _chargeForce = new Force(speed, direction, 1, Force.ForceMode.INPUT, AnimationCurve.Linear(1f, 1f, 1f, 1f), 0.1f, AnimationCurve.Linear(0f, 0f, 0f, 0f), 0);
        while (!_entityBoss.GetAnimationBool("CanCharge")) {
            yield return null;
        }
        _entityPhysics.Add(_chargeForce, 1);
        while (!_hitWall) {
            yield return null;
        }
        _entityBoss.SetAnimationBool("HitWall", true);
        _entityBoss.SetAnimationBool("Charging", false);
        _entityBoss.SetAnimationBool("CanCharge", false);
        _entityPhysics.Remove(_chargeForce);
        _bounceWallForce.Direction = _bounceWallDirection;
        _entityPhysics.Add(new Force(_bounceWallForce), 1);
        yield return new WaitForSeconds(_bounceWallForce.Duration);
    }

    protected IEnumerator ChargeDestination(Vector3 destination, float delay, float speed) {
        yield return new WaitForSeconds(delay);
        Vector3 direction = destination - _transform.position;
        if (Vector3.Dot(Vector3.right, direction) < 0) {
            _entityBoss.FlipRight(false);
        } else {
            _entityBoss.FlipRight(true);
        }
        bool pass = false;
        float lastDist = Vector3.Distance(destination, _transform.position);
        _chargeForce = new Force(speed, direction, 1);
        _entityBoss.SetAnimationBool("Charging", true);
        while (!_entityBoss.GetAnimationBool("CanCharge")) {
            yield return null;
        }
        _entityPhysics.Add(_chargeForce, 1);
        while (!pass) {
            float dot = Vector3.Dot(direction, destination - _transform.position);
            if (dot < 0) {
                pass = true;
                _transform.position = destination;
                _entityBoss.SetAnimationBool("HitWall", false);
                _entityBoss.SetAnimationBool("Charging", false);
                _entityBoss.SetAnimationBool("CanCharge", false);
                _entityPhysics.Remove(_chargeForce);
            }
            yield return null;
        }
    }

    protected void Hit(Collision2D collision) {
        if (collision.transform.CompareTag("Wall")) {
            _hitWall = true;
            _bounceWallDirection = collision.contacts[0].normal.normalized;
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