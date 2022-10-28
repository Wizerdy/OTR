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
    protected override IEnumerator Attack(EntityAbilities ea, Transform target) {
        _hitWall = false;
        _entityMovement = ea.Get<EntityMovement>();
        _entityColliders = ea.Get<EntityColliders>();
        _entityColliders.Main.OnTriggerEnter += Hit;
        yield return StartCoroutine(Charge(ea.transform.position, target.position, _delayBeforeCharge, _speed));
        _entityMovement.StopMovement();
        _entityColliders.Main.OnTriggerEnter -= Hit;
        yield return StartCoroutine(Disactivate());
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
            Debug.Log("hit");
            _hitWall = true;
        }
    }

    protected void Hit(Collider2D collision) {
        if (collision.transform.tag == "Wall") {
            Debug.Log("hit");
            _hitWall = true;

        }
    }
}