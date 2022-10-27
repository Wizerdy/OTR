using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossCharge : BossAttack {
    [Header("Charge :")]
    [SerializeField] protected float _speed;
    [SerializeField] protected float _duration;
    [SerializeField] protected float _delayCharge;
    protected bool _hitWall = false;
    protected EntityMovement _entityMovement;
    protected override IEnumerator Attack(EntityAbilities ea, Transform target) {
        _entityMovement = ea.Get<EntityMovement>();
        yield return StartCoroutine(Charge(ea.gameObject.transform.position, target.position));
    }

    public void Hit(Collision2D collision) {
        if(collision.transform.tag == "Wall") {
            _entityMovement.StopMovement();
        }
    }

    protected IEnumerator Charge(Vector3 ownPosition, Vector3 targetPosition) {
        _entityMovement.CreateMovement(_duration, _speed, targetPosition - ownPosition);
        while (!_hitWall) {
            yield return null;
        }
    }
}
