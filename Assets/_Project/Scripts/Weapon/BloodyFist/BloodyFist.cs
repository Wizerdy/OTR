using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class BloodyFist : Weapon {

    [SerializeField] int _comboIndex = 0;
    string[] _triggerName = { "FistHitRight", "FistHitLeft" };
    EntityStorePoint _entityStorePoint;
    EntityMovement _entityMovement;

    [Header("General")]
    [SerializeField] float _healthPercentValueStorePoint;
    [SerializeField] float _storePointMax;

    [Header("Fist")]
    [SerializeField] float _hitdamage;
    [SerializeField] float _hitcooldown;
    [SerializeField] float _hitThreatPoint;
    [SerializeField] float _hitStorePoint;
    [SerializeField] float _hitStorePointCost;
    [SerializeField] float _pushduration;
    [SerializeField] float _pushStrenght;
    [SerializeField] AnimationCurve _pushCurve;

    [Header("Dash")]
    [SerializeField] float _storePointCost;
    [SerializeField] float _dashCooldown;
    [SerializeField] float _speed;
    [SerializeField] float _duration;
    [SerializeField] AnimationCurve _accelerationCurve;
    Timer _cooldown;
    protected override void _OnStart() {
        _attacks.Add(AttackIndex.FIRST, FirstAttack);
        _attacks.Add(AttackIndex.SECOND, SecondAttack);
        _cooldown = new Timer(CoroutinesManager.Instance, _hitcooldown, false);
    }

    protected override void _OnDrop(EntityWeaponry entityWeaponry) {
        _entityStorePoint = null;
        _entityMovement = null;
        _comboIndex = 1;
    }
    protected IEnumerator FirstAttack(EntityAbilities ea, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        if (_cooldown.IsWorking) { yield break; }
        if (_entityStorePoint == null) {
            _entityStorePoint = ea.Get<EntityStorePoint>();
            _entityStorePoint.ChangeMinValue(0f);
            _entityStorePoint.ChangeMaxValue(_storePointMax);
        }
        _cooldown.Duration = _hitcooldown;
        _cooldown.Start();
        _targetAnimator.SetTrigger(_triggerName[_comboIndex]);
        ++_comboIndex;
        _comboIndex %= _triggerName.Length;
        yield return null;
    }

    protected IEnumerator SecondAttack(EntityAbilities ea, Vector2 direction) {
        if (_cooldown.IsWorking) { yield break; }
        if (_entityMovement == null) {
            _entityStorePoint = ea.Get<EntityStorePoint>();
            _entityStorePoint.ChangeMinValue(0f);
            _entityStorePoint.ChangeMaxValue(_storePointMax);
            _entityMovement = ea.Get<EntityMovement>();
        }
        _cooldown.Duration = _dashCooldown;
        _cooldown.Start();
        _entityMovement.CreateMovement(_duration, _speed, default, _accelerationCurve);
        _entityStorePoint.LosePoint(_storePointCost, true);
        yield return null;
    }

    protected override void _OnAttackHit(Collider2D collider) {
        Debug.Log(collider.transform.gameObject);
        if (collider.tag == "Boss" || collider.tag == "Enemy") {
            _entityStorePoint.GainPoint(_hitStorePoint);
        }
        if (collider.tag == "Player" && collider.gameObject.GetRoot() != User.gameObject) {
            Debug.Log(collider.gameObject.transform.parent);
            collider.gameObject.GetComponentInRoot<IHealth>()?.TakeHeal((int)(_hitStorePointCost * _healthPercentValueStorePoint));
            _entityStorePoint.LosePoint(_hitStorePointCost, false);
            collider.gameObject.GetRoot().GetComponentInChildren<EntityMovement>()?.CreateMovement(_pushduration, _pushStrenght, collider.gameObject.transform.position - transform.position, _pushCurve);
        }
    }
}
