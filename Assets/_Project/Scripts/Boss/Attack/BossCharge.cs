using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossCharge : BossAttack {
    [Header("Charge :")]
    [SerializeField] protected float _delayBeforeCharge;
    [SerializeField] protected float _duration;
    [SerializeField] protected float _speed;
    protected bool _hitWall = false;
    protected EntityMovement _entityMovement;
    protected EntityColliders _entityColliders;
    protected DamageHealth _damageHealth;
    protected int _damagesMemory;

    protected override IEnumerator AttackBegins(EntityAbilities ea, Transform target) {
        _hitWall = false;
        _entityMovement = ea.Get<EntityMovement>();
        _entityColliders = ea.Get<EntityColliders>();
        _damageHealth = _entityColliders.Main.gameObject.GetComponent<DamageHealth>();
        _damagesMemory = _damageHealth.Damage;
        _damageHealth.SetDamage(_damages);
        _entityColliders.Main.OnTriggerEnter += Hit;
        yield break;
    }
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        yield return StartCoroutine(Charge(ea.transform.position, target.position, _delayBeforeCharge, _speed));
    }

    protected override IEnumerator AttackEnds(EntityAbilities ea, Transform target) {
        _entityMovement.StopMovement();
        _damageHealth.SetDamage(_damagesMemory);
        _entityColliders.Main.OnTriggerEnter -= Hit;
        yield break;
    }

    protected IEnumerator Charge(Vector3 ownPosition, Vector3 targetPosition, float delay, float speed) {
        yield return new WaitForSeconds(delay);
        _entityMovement.CreateMovement(_duration, speed, targetPosition - ownPosition);
        while (!_hitWall) {
            yield return null;
        }
    }

    protected IEnumerator ChargeDestination(Transform transform, Vector3 destination, float delay, float speed) {
        yield return new WaitForSeconds(delay);
        _entityMovement.CreateMovement(_duration, speed, destination - transform.position);
        bool pass = false;
        float lastDist = Vector3.Distance(destination, transform.position);
        while (!pass) {
            float currentDist = Vector3.Distance(destination, transform.position);
            if (currentDist > lastDist) {
                pass = true;
            } else {
                lastDist = currentDist;
            }
            yield return null;
        }
    }

    protected void Hit(Collision2D collision) {
        if (collision.transform.tag == "Wall") {
            _hitWall = true;
        }
    }

    protected void Hit(Collider2D collision) {
        if (collision.transform.tag == "Wall") {
            _hitWall = true;

        }
    }
}