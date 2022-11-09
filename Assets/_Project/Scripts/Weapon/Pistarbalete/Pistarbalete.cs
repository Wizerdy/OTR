using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Pistarbalete : Weapon {
    [Header("Primary Attack")]
    [SerializeField] GameObject _bolt;
    [SerializeField] float _attackTime = 0.2f;
    [SerializeField] int _boltDamages = 5;
    //[SerializeField] int _boltDamageBonus = 2;
    [SerializeField] float _boltCooldown = 1f;
    [SerializeField] float _boltSpeed = 5f;
    [SerializeField] int _threatPoint = 5;

    [Header("Secondary Attack")]
    [SerializeField] GameObject specialBolt;
    [SerializeField] PowerUp _powerUp;
    [SerializeField] float _sbAttackTime = 0.0f;
    [SerializeField] float _sbCooldown = 0.2f;
    [SerializeField] float _sbSpeed = 5f;
    [SerializeField] int _sbThreatPoint = 5;

    bool _aiming = false;

    protected override void _OnStart() {
        _type = WeaponType.CROSSGUN;
        _attacks.Add(AttackIndex.FIRST, new WeaponAttack(_attackTime, _boltDamages, _threatPoint, INormalShoot));
        _attacks.Add(AttackIndex.SECOND, new WeaponAttack(_sbAttackTime, 0, _sbThreatPoint, IBuffShoot));
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
        //if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        //_targetAnimator.SetTrigger(_triggerName);
        GameObject go = Instantiate(_bolt, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
        //go.GetComponent<Rigidbody2D>().velocity = direction * _boltSpeed;
        go.GetComponent<Bolt>()?.SetDamage(_boltDamages)?.SetSpeed(_boltSpeed)?.SetDirection(direction);
        //go.GetComponent<AditionalDamageByDistance>().PerfectDamage = _boltDamageBonus;
        DamageHealth dh = go.GetComponent<DamageHealth>();
        if (dh != null) {
            dh.DamageModifier.CopyReference(entityAbilities.Get<EntityWeaponry>().DamageHealth.DamageModifier);
            dh.OnDamage += _InvokeFirstAttackHit;
        }

        _canAttack = false;
        CoroutinesManager.Start(Tools.Delay(() => _canAttack = true, _boltCooldown));

        if (_attackTime > 0) {
            yield return new WaitForSeconds(_attackTime);
        }
    }

    protected IEnumerator IBuffShoot(EntityAbilities entityAbilities, Vector2 direction) {
        EntityPhysicMovement movements = entityAbilities.Get<EntityPhysicMovement>();

        if (_aiming || (movements?.Moving ?? false)) {
            GameObject go = Instantiate(specialBolt, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
            go.GetComponent<Rigidbody2D>().velocity = direction * _sbSpeed;
            go.GetComponent<DamageHealth>()?.Hitted.Add(entityAbilities.gameObject);
        } else {
            EntityPowerUp pu = entityAbilities.Get<EntityPowerUp>();
            if (pu != null) {
                pu.Add(_powerUp);
            } else {
                Debug.LogError("No Entity Power Up, can't Power Up");
            }
        }

        if (_attackTime > 0) {
            yield return new WaitForSeconds(_sbAttackTime);
        }
    }

    private void _InvokeFirstAttackHit(IHealth health, int damage) {
        _onAttackHit.Invoke(AttackIndex.FIRST, health, damage);
    }
}
