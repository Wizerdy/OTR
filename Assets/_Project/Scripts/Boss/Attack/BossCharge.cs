using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine.BetterEvents;

public class BossCharge : BossAttack {
    [Header("Charge :")]
    [SerializeField] protected float _speed;
    [SerializeField] protected Force _bounceForce;
    [SerializeField] protected Force _bounceWallForce;
    protected bool _hitWall = false;
    protected Vector3 _hitPosition;
    protected DamageHealth _damageHealth;
    protected Force _chargeForce;
    protected int _damagesMemory;
    protected Vector3 _bounceWallDirection;
    protected float angleTopBotCharge = 44;
    protected float angleSideCharge = 92;
    protected float angleTopBotHit = 80;
    protected float angleSideHit = 20;
    protected bool didbegin = false;
    [SerializeField] BetterEvent _onCharge = new BetterEvent();
    [SerializeField] BetterEvent _onChargeBack = new BetterEvent();

    public event UnityAction OnCharge { add => _onCharge += value; remove => _onCharge -= value; }
    public event UnityAction OnChargeBack { add => _onChargeBack += value; remove => _onChargeBack -= value; }

    public override void Disable() {
        base.Disable();
        if (didbegin) {
            _damageHealth.SetDamage(_damagesMemory);
            _entityColliders.MainEvent.OnCollisionEnter -= Hit;
        }
    }
    protected override IEnumerator AttackBegins(EntityAbilities ea, Transform target) {
        _hitWall = false;
        _damageHealth = _entityColliders.MainEvent.gameObject.GetComponent<DamageHealth>();
        _damageHealth.ResetHitted();
        _damagesMemory = _damageHealth.Damage;
        _damageHealth.SetDamage(_damages);
        _entityColliders.MainEvent.OnCollisionEnter += Hit;
        didbegin = true;
        yield break;
    }
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        yield return StartCoroutine(Charge(target.position, _speed));
    }

    protected override IEnumerator AttackEnds(EntityAbilities ea, Transform target) {
        _damageHealth.SetDamage(_damagesMemory);
        _entityColliders.MainEvent.OnCollisionEnter -= Hit;
        yield break;
    }

    protected IEnumerator Charge(Vector3 targetPosition, float speed) {
        Vector3 direction = (targetPosition - _transform.position).normalized;
        float angle = Vector2.Angle(Vector2.up, direction);
        if (angle < angleTopBotCharge) {
            _entityBoss.SetAnimationBool("ChargingTop", true);
        } else if (angle < angleTopBotCharge + angleSideCharge) {
            _entityBoss.SetAnimationBool(Vector3.Dot(Vector3.right, direction) < 0 ? "ChargingLeft" : "ChargingRight", true);
        } else {
            _entityBoss.SetAnimationBool("ChargingBot", true);
        }
        _chargeForce = new Force(speed, direction, 1, Force.ForceMode.INPUT, AnimationCurve.Linear(1f, 1f, 1f, 1f), 0.1f, AnimationCurve.Linear(0f, 0f, 0f, 0f), 0);
        while (!_entityBoss.GetAnimationBool("CanCharge")) {
            yield return null;
        }
        _entityPhysics.Add(_chargeForce, 1);
        _onCharge?.Invoke();
        while (!_hitWall) {
            yield return null;
        }

        //yield return new WaitForSeconds(0.05f);
        //do {
        //    yield return null;
        //} while (_entityPhysics.Velocity.magnitude > 0.01f);

        if (angle < angleTopBotCharge) {
            _entityBoss.SetAnimationBool("ChargingTop", false);
        } else if (angle < angleTopBotCharge + angleSideCharge) {
            _entityBoss.SetAnimationBool(Vector3.Dot(Vector3.right, direction) < 0 ? "ChargingLeft" : "ChargingRight", false);
        } else {
            _entityBoss.SetAnimationBool("ChargingBot", false);
        }
        angle = Vector2.Angle(Vector2.up, _hitPosition);
        //Debug.Log(angle);
        //Debug.Log(_hitPosition);
        if (angle < angleTopBotHit) {
            _entityBoss.SetAnimationTrigger("HitTop");
        } else if (angle < angleTopBotHit + angleSideHit) {
            _entityBoss.SetAnimationTrigger(Vector3.Dot(Vector3.right, direction) < 0 ? "HitLeft" : "HitRight");
        } else {
            _entityBoss.SetAnimationTrigger("HitBot");
        }
        _entityBoss.SetAnimationBool("CanCharge", false);
        _entityPhysics.Remove(_chargeForce);
        _bounceWallForce.Direction = _bounceWallDirection;
        _entityPhysics.Add(new Force(_bounceWallForce), 1);
        yield return new WaitForSeconds(_bounceWallForce.Duration);
    }

    protected IEnumerator ChargeDestination(Vector3 destination, float speed) {
        Vector3 direction = (destination - _transform.position).normalized;
        float angle = Vector2.Angle(Vector2.up, direction);
        if (angle < angleTopBotCharge) {
            _entityBoss.SetAnimationBool("ChargingTop", true);
        } else if (angle < angleTopBotCharge + angleSideCharge) {
            _entityBoss.SetAnimationBool(Vector3.Dot(Vector3.right, direction) < 0 ? "ChargingLeft" : "ChargingRight", true);
        } else {
            _entityBoss.SetAnimationBool("ChargingBot", true);
        }
        _chargeForce = new Force(speed, direction, 1, Force.ForceMode.INPUT, AnimationCurve.Linear(1f, 1f, 1f, 1f), 0.1f, AnimationCurve.Linear(0f, 0f, 0f, 0f), 0);
        while (!_entityBoss.GetAnimationBool("CanCharge")) {
            yield return null;
        }
        _entityPhysics.Add(_chargeForce, 1);
        bool pass = false;
        float dot = 0;
        _onChargeBack?.Invoke();
        while (!pass) {
            yield return null;
            dot = Vector3.Dot(direction, destination - _transform.position);
            if (dot < 0) {
                pass = true;
            }
        }

        if (angle < angleTopBotCharge) {
            _entityBoss.SetAnimationBool("ChargingTop", false);
        } else if (angle < angleTopBotCharge + angleSideCharge) {
            _entityBoss.SetAnimationBool(Vector3.Dot(Vector3.right, direction) < 0 ? "ChargingLeft" : "ChargingRight", false);
        } else {
            _entityBoss.SetAnimationBool("ChargingBot", false);
        }
        _entityBoss.SetAnimationBool("CanCharge", false);
        _entityPhysics.Remove(_chargeForce);
        _transform.position = destination;
    }

    protected void Hit(Collision2D collision) {
        Debug.Log("hit!!");
        if (collision.transform.CompareTag("Wall")) {
            _hitWall = true;
            _bounceWallDirection = collision.contacts[0].normal.normalized;
            _hitPosition = (V.ToVector3(collision.contacts[0].point) - collision.otherCollider.transform.position).normalized;
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
            eiPlayer.ChangeCollisionLayer(_bounceForce.Duration);
            epPlayer.Add(new Force(_bounceForce), 10);
        }
    }
    protected void HitExit(Collision2D collision) {
        if (collision.transform.CompareTag("Wall")) {
            _hitWall = false;
        }
    }

    public static class V {
        public static Vector3 ToVector3(Vector2 v) {
            return new Vector3(v.x, v.y, 0);
        }
    }
}