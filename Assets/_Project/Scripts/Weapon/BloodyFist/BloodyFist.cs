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

    [Header("Dash")]
    [SerializeField] float _storePointCost;
    [SerializeField] float _cooldown;
    [SerializeField] float _speed;
    [SerializeField] float _duration;
    [SerializeField] AnimationCurve _accelerationCurve;

    protected override void _OnStart() {
        _attacks.Add(AttackIndex.FIRST, FirstAttack);
        _attacks.Add(AttackIndex.SECOND, SecondAttack);
    }

    protected override void _OnDrop(EntityWeaponry entityWeaponry) {
        _entityStorePoint = null;
        _entityMovement = null;
        _comboIndex = 1;
    }
    protected IEnumerator FirstAttack(EntityAbilities ea, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        if (_entityStorePoint == null)
            _entityStorePoint = ea.Get<EntityStorePoint>();
        _targetAnimator.SetTrigger(_triggerName[_comboIndex]);
        ++_comboIndex;
        _comboIndex %= _triggerName.Length;
        yield return null;
    }

    protected IEnumerator SecondAttack(EntityAbilities ea, Vector2 direction) {
        if (_entityMovement == null)
            _entityMovement = ea.Get<EntityMovement>();

        _entityMovement.CreateMovement(_duration, _speed, _accelerationCurve);
        yield return null;
    }

    protected override void _OnAttackHit(Collider2D collider) {
        if (collider.tag == "Boss" || collider.tag == "Enemy") {
            collider.gameObject.GetComponent<IHealth>().TakeDamage((int)_hitdamage, gameObject);
            _entityStorePoint.GainStorePoint(_hitStorePoint);
        }
        if (collider.tag == "Player") {
            collider.gameObject.GetComponent<IHealth>().TakeHeal((int)_hitStorePointCost);
            _entityStorePoint.LoseStorePoint(_storePointCost);
        }
    }
}
