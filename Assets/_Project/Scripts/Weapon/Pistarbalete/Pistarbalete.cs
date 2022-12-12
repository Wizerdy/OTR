using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Pistarbalete : Weapon {
    [Header("Primary Attack")]
    [SerializeField] GameObject _bolt;
    [SerializeField] float _attackTime = 0.2f;
    [SerializeField] int _boltDamages = 5;
    [SerializeField] float _boltCooldown = 1f;
    [SerializeField] float _boltSpeed = 5f;
    [SerializeField] int _threatPoint = 5;

    [Header("Distance damage")]
    [SerializeField] TransformReference _bossPosition;
    [SerializeField] AnimationCurve _damageByDistance = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] int _maxDamage = 7;
    [SerializeField] int _perfectDamage = 0;
    [SerializeField] Vector4 _radiuses = new Vector4(1f, 3f, 5f, 7f);

    [Header("Secondary Attack")]
    [SerializeField] GameObject specialBolt;
    [SerializeField] PowerUp _powerUp;
    [SerializeField] float _sbAttackTime = 0.0f;
    [SerializeField] float _sbCooldown = 0.2f;
    [SerializeField] float _sbSpeed = 5f;
    [SerializeField] int _sbThreatPoint = 5;

    bool _aiming = false;
    string _attackTriggerName = "Crossgun_Attack";
    string _boostTriggerName = "Crossgun_Boost";

    Timer _attackCooldown;
    Timer _specialAttackCooldown;

    protected override void _OnStart() {
        _type = WeaponType.CROSSGUN;
        _attacks.Add(AttackIndex.FIRST, new WeaponAttack(_attackTime, _boltDamages, _threatPoint, INormalShoot));
        _attacks.Add(AttackIndex.SECOND, new WeaponAttack(_sbAttackTime, 0, _sbThreatPoint, IBuffShoot));

        _attackCooldown = new Timer(CoroutinesManager.Instance, _boltCooldown, false);
        _attackCooldown.OnEnd += (() => { _attacks[AttackIndex.FIRST].canAttack = true; });
        _specialAttackCooldown = new Timer(CoroutinesManager.Instance, _sbCooldown, false);
        _specialAttackCooldown.OnEnd += (() => { _attacks[AttackIndex.SECOND].canAttack = true; });
    }

    protected override void _OnAim(Vector2 direction) {
        if (direction == Vector2.zero) {
            if (_aiming) { StopAiming(); }
            return;
        }

        if (!_aiming) { StartAiming(); }
        UpdateAim(direction);
    }

    #region Aiming

    private void StartAiming() {
        _aiming = true;
    }

    private void UpdateAim(Vector2 direction) {

    }

    private void StopAiming() {
        _aiming = false;
    }

    #endregion

    protected IEnumerator INormalShoot(EntityAbilities entityAbilities, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        if (_attackCooldown.IsWorking) { yield break; }

        _targetAnimator.SetTrigger(_attackTriggerName);

        GameObject go = Instantiate(_bolt, transform.position, Quaternion.LookRotation(Vector3.forward, direction));

        float percentage = ComputePercentage(_bossPosition.IsValid() ? _bossPosition.Instance.Position2D() : Vector2.zero);
        int damage = ComputeDamage(percentage);

        go.GetComponent<Bolt>()?
            .SetDamage(_boltDamages + damage)?
            .SetSpeed(_boltSpeed)?
            .SetDirection(direction)?
            .SetPercentage(percentage);

        DamageHealth dh = go.GetComponent<DamageHealth>();
        if (dh != null) {
            dh.DamageModifier.CopyReference(entityAbilities.Get<EntityWeaponry>().DamageHealth.DamageModifier);
            dh.OnDamage += _InvokeFirstAttackHit;
        }

        _attacks[AttackIndex.FIRST].canAttack = false;
        _attackCooldown.Start();

        if (_attackTime > 0) {
            yield return new WaitForSeconds(_attackTime);
        }
    }

    private float ComputePercentage(Vector2 target) {
        float distance = (target - transform.Position2D()).magnitude;
        float percentage;

        if (distance > _radiuses.y && distance < _radiuses.z) {
            percentage = 1f;
        } else {
            percentage = (distance - _radiuses.x) / (_radiuses.y - _radiuses.x);
            if (percentage > 1f) {
                percentage = 1f - ((distance - _radiuses.z) / (_radiuses.w - _radiuses.z));
            }
        }

        percentage = Mathf.Max(0f, Mathf.Min(1f, percentage));
        return percentage;
    }

    private int ComputeDamage(float percentage) {
        return Mathf.CeilToInt(_maxDamage * _damageByDistance.Evaluate(percentage)) + (percentage >= 1f ? _perfectDamage : 0);
    }

    protected IEnumerator IBuffShoot(EntityAbilities entityAbilities, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        if (_specialAttackCooldown.IsWorking) { yield break; }

        _targetAnimator.SetTrigger(_boostTriggerName);

        EntityPhysicMovement movements = entityAbilities.Get<EntityPhysicMovement>();

        if (_aiming || (movements?.Moving ?? false)) {
            GameObject go = Instantiate(specialBolt, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
            go.GetComponent<Rigidbody2D>().velocity = direction * _sbSpeed;
            go.GetComponent<DamageHealth>()?.IgnoreCollisionOnce(entityAbilities.gameObject);
        } else {
            EntityPowerUp pu = entityAbilities.Get<EntityPowerUp>();
            if (pu != null) {
                pu.Add(_powerUp);
            } else {
                Debug.LogError("No Entity Power Up, can't Power Up");
            }
        }

        _attacks[AttackIndex.SECOND].canAttack = false;
        _specialAttackCooldown.Start();

        if (_attackTime > 0) {
            yield return new WaitForSeconds(_sbAttackTime);
        }
    }

    private void _InvokeFirstAttackHit(IHealth health, int damage) {
        _onAttackHit.Invoke(AttackIndex.FIRST, health, damage);
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _radiuses.x);
        Gizmos.DrawWireSphere(transform.position, _radiuses.w);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, _radiuses.y);
        Gizmos.DrawWireSphere(transform.position, _radiuses.z);
    }
}
