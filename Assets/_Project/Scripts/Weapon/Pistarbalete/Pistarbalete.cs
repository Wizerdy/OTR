using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Pistarbalete : Weapon {
    [Header("Primary Attack")]
    [SerializeField] float _attackTime = 0.2f;
    [Header("Bolt")]
    [SerializeField] GameObject _bolt;
    [SerializeField] int _boltDamage = 5;
    [SerializeField] int _boltDamageBonus = 2;
    [SerializeField] float _boltCooldown = 1;
    [SerializeField] float _boltSpeed = 5f;

    [Header("Secondary Attack")]
    [SerializeField] GameObject specialBolt;
    [SerializeField] PowerUp _powerUp;
    [SerializeField] float _sbAttackTime = 2.0f;
    [SerializeField] float _sbSpeed = 5f;

    bool _aiming = false;

    protected override void _OnStart() {
        _type = WeaponType.CROSSGUN;
        _attacks.Add(AttackIndex.FIRST, INormalShoot);
        _attacks.Add(AttackIndex.SECOND, IBuffShoot);
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
        go.GetComponent<Bolt>()?.SetDamage(_boltDamage)?.SetSpeed(_boltSpeed)?.SetDirection(direction);
        go.GetComponent<AditionalDamageByDistance>().PerfectDamage = _boltDamageBonus;
        go.GetComponent<DamageHealth>()?.DamageModifier.CopyReference(entityAbilities.Get<EntityWeaponry>().DamageHealth.DamageModifier);
        _canAttack = false;
        CoroutinesManager.Start(Tools.Delay(() => _canAttack = true, _boltCooldown));
        if (_attackTime > 0) {
            yield return new WaitForSeconds(_attackTime);
        }
    }

    protected IEnumerator IBuffShoot(EntityAbilities entityAbilities, Vector2 direction) {
        //if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        //_targetAnimator.SetTrigger(_triggerName);
        if (_aiming) {
            GameObject go = Instantiate(specialBolt, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
            go.GetComponent<Rigidbody2D>().velocity = direction * _sbSpeed;
        } else {
            _powerUp = _powerUp.SetTarget(entityAbilities);
            _powerUp.Enable();
        }

        if (_attackTime > 0) {
            yield return new WaitForSeconds(_sbAttackTime);
        }
    }

    public override float AttackTime(AttackIndex index) {
        switch (index) {
            case AttackIndex.FIRST:
                return _attackTime;
            case AttackIndex.SECOND:
                return _sbAttackTime;
            default:
                return 0f;
        }
    }
}
