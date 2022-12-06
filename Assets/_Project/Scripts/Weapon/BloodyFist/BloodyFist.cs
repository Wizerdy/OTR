using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class BloodyFist : Weapon {

    [SerializeField] int _comboIndex = 0;
    string[] _triggerName = { "Fist_Hit_Right", "Fist_Hit_Left" };
    EntityStorePoint _entityStorePoint;
    EntityPhysics _entityPhysics;

    [Header("General")]
    [SerializeField] float _healthPercentValueStorePoint;
    [SerializeField] float _storePointMax;

    [Header("Fist")]
    [SerializeField] int _hitDamage = 10;
    [SerializeField] int _hitThreatPoint = 10;
    [SerializeField] float _hitCooldown = 0.15f;
    [SerializeField] float _hitStorePoint = 1f;
    [SerializeField] float _hitStorePointCost = 5f;
    //[SerializeField] float _pushDuration;
    //[SerializeField] float _pushStrength = 5f;
    //[SerializeField] float _pushAttackTime = 0.2f;
    //[SerializeField] AnimationCurve _pushCurve;
    [SerializeField] ForceData _bumpForce;

    [Header("Dash")]
    [SerializeField] float _storePointCost;
    [SerializeField] float _dashCooldown;
    [SerializeField] ForceData _dashForce;
    [SerializeField] float _dashWeight = 0.8f;

    Force _dash;
    Vector2 _lastFistDirection = Vector2.zero;
    //[SerializeField] float _speed;
    //[SerializeField] float _duration;
    //[SerializeField] AnimationCurve _accelerationCurve;

    Timer _cooldown;

    public float BloodPointsOnHit { get => _hitStorePoint; set => _hitStorePoint = value; }

    protected override void _OnStart() {
        _attacks.Add(AttackIndex.FIRST, new WeaponAttack(_hitCooldown, _hitDamage, _hitThreatPoint, FistAttack));
        _attacks.Add(AttackIndex.SECOND, new WeaponAttack(0f, 0, 0, BloodyDash));
        _dash = new Force(_dashForce, Vector2.zero, _dashWeight, Force.ForceMode.TIMED);
        _cooldown = new Timer(CoroutinesManager.Instance, _hitCooldown, false);
        _type = WeaponType.BLOODFIST;
    }

    protected override void _OnDrop(EntityWeaponry entityWeaponry) {
        _entityStorePoint = null;
        _entityPhysics = null;
        _comboIndex = 1;
    }

    protected IEnumerator FistAttack(EntityAbilities ea, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        if (_cooldown.IsWorking) { yield break; }
        if (_entityStorePoint == null) {
            _entityStorePoint = ea.Get<EntityStorePoint>();
            _entityStorePoint.ChangeMinValue(0f);
            _entityStorePoint.ChangeMaxValue(_storePointMax);
        }

        if (direction != Vector2.zero) { _targetAnimator?.SetFloat("x", direction.x); _targetAnimator?.SetFloat("y", direction.y); }
        _lastFistDirection = direction;
        _cooldown.Duration = _hitCooldown;
        _cooldown.Start();
        _targetAnimator.SetTrigger(_triggerName[_comboIndex]);
        ++_comboIndex;
        _comboIndex %= _triggerName.Length;
        yield return new WaitForSeconds(_hitCooldown);
    }

    protected IEnumerator BloodyDash(EntityAbilities ea, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        if (_cooldown.IsWorking) { yield break; }
        if (_entityPhysics == null) {
            _entityPhysics = ea.Get<EntityPhysics>();
        }
        if (_entityStorePoint == null) {
            _entityStorePoint = ea.Get<EntityStorePoint>();
            _entityStorePoint.ChangeMinValue(0f);
            _entityStorePoint.ChangeMaxValue(_storePointMax);
        }
        _targetAnimator.SetBool("BloodFist_Dash", true);
        _cooldown.Duration = _dashCooldown;
        _cooldown.Start();

        _dash.Reset();
        _dash.Direction = direction;
        _entityPhysics.Add(_dash, (int)PhysicPriority.DASH);
        //_entityMovement?.CreateMovement(_duration, _speed, default, _accelerationCurve);
        _entityStorePoint.LosePoint(_storePointCost, true);
        yield return new WaitForSeconds(_dash.Duration);
        _targetAnimator?.SetBool("BloodFist_Dash", false);
    }

    protected override void _OnAttackTrigger(Collider2D collider) {
        //Debug.Log(collider.transform.gameObject);
        if (collider.CompareTag("Boss") || collider.CompareTag("Enemy")) {
            //Debug.Log("touché");
            _entityStorePoint.GainPoint(_hitStorePoint);
        }
        if (collider.CompareTag("Player") && collider.gameObject.GetRoot() != User.gameObject /*&& !collider.gameObject.GetRoot().transform.IsChildOf(User.gameObject.transform)*/) {
            collider.gameObject.GetComponentInRoot<IHealth>()?.TakeHeal((int)(_hitStorePointCost * _healthPercentValueStorePoint));
            _entityStorePoint.LosePoint(_hitStorePointCost, false);
            //collider.gameObject.GetRoot().GetComponentInChildren<EntityMovement>()?.CreateMovement(_pushDuration, _pushStrenght, collider.gameObject.transform.position - transform.position, _pushCurve);
            //Vector2 bumpDirection = (collider.gameObject.transform.position - transform.position).normalized;
            collider.gameObject.GetComponentInRoot<EntityAbilities>()?
                .Get<EntityPhysics>()?
                .Add(new Force(_bumpForce, _lastFistDirection, 0.7f, Force.ForceMode.TIMED), (int)PhysicPriority.PROJECTION);
        }
    }
}
