using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using TMPro;

public class BloodyFist : Weapon {

    [SerializeField] int _comboIndex = 0;
    string[] _triggerName = { "Fist_Hit_Right", "Fist_Hit_Left" };
    EntityStorePoint _entityStorePoint;
    EntityPhysics _entityPhysics;

    [Header("General")]
    [SerializeField, Range(0f, 1f)] float _healthPercentRegen;
    [SerializeField] float _storePointMax;

    [Header("Fist")]
    [SerializeField] int _hitDamage = 10;
    [SerializeField] int _hitThreatPoint = 10;
    [SerializeField] float _hitCooldown = 0.15f;
    [SerializeField] float _hitDuration = 0.15f;
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

    Timer _fistTimer;
    Timer _dashTimer;

    List<Collider2D> _healedColliders = new List<Collider2D>();

    public float BloodPointsOnHit { get => _hitStorePoint; set => _hitStorePoint = value; }

    protected override void _OnPickup(EntityWeaponry weaponry) {
        weaponry.DamageHealth.OnDamage += GainBloodPoint;
    }

    protected override void _OnStart() {
        _attacks.Add(AttackIndex.FIRST, new WeaponAttack(_hitDuration, _hitDamage, _hitThreatPoint, FistAttack));
        _attacks.Add(AttackIndex.SECOND, new WeaponAttack(0f, 0, 0, BloodyDash));
        _dash = new Force(_dashForce, Vector2.zero, _dashWeight, Force.ForceMode.TIMED);
        _fistTimer = new Timer(CoroutinesManager.Instance, _hitCooldown, false);
        _fistTimer.OnEnd += (() => { _attacks[AttackIndex.FIRST].canAttack = true; });
        _dashTimer = new Timer(CoroutinesManager.Instance, _dashCooldown, false);
        _dashTimer.OnEnd += (() => { _attacks[AttackIndex.SECOND].canAttack = true; });
        _type = WeaponType.BLOODFIST;
    }

    protected override void _OnDrop(EntityWeaponry entityWeaponry) {
        _targetAnimator?.SetBool("BloodFist_Dash", false);
        _entityStorePoint = null;
        _entityPhysics = null;
        _comboIndex = 1;
        entityWeaponry.DamageHealth.OnDamage -= GainBloodPoint;
    }

    protected IEnumerator FistAttack(EntityAbilities ea, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        if (_fistTimer.IsWorking) { yield break; }
        if (_entityStorePoint == null) {
            _entityStorePoint = ea.Get<EntityStorePoint>();
            _entityStorePoint.ChangeMinValue(0f);
            _entityStorePoint.ChangeMaxValue(_storePointMax);
        }

        _healedColliders.Clear();
        if (direction != Vector2.zero) { _targetAnimator?.SetFloat("x", direction.x); _targetAnimator?.SetFloat("y", direction.y); }
        _lastFistDirection = direction;
        _attacks[AttackIndex.FIRST].canAttack = false;
        _fistTimer.Start();
        _targetAnimator.SetTrigger(_triggerName[_comboIndex]);
        ++_comboIndex;
        _comboIndex %= _triggerName.Length;
        yield return new WaitForSeconds(_hitDuration);
    }

    protected IEnumerator BloodyDash(EntityAbilities ea, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        if (_dashTimer.IsWorking) { yield break; }
        if (_entityPhysics == null) {
            _entityPhysics = ea.Get<EntityPhysics>();
        }
        if (_entityStorePoint == null) {
            _entityStorePoint = ea.Get<EntityStorePoint>();
            _entityStorePoint.ChangeMinValue(0f);
            _entityStorePoint.ChangeMaxValue(_storePointMax);
        }
        ea.Get<EntityInvincibility>()?.ChangeCollisionLayer(_dash.Duration);
        _targetAnimator.SetBool("BloodFist_Dash", true);
        _attacks[AttackIndex.SECOND].canAttack = false;
        _dashTimer.Start();

        _dash.Reset();
        _dash.Direction = direction;
        _entityPhysics.Add(_dash, (int)PhysicPriority.DASH);
        _attacks[AttackIndex.SECOND].canAttack = false;
        //_entityMovement?.CreateMovement(_duration, _speed, default, _accelerationCurve);
        _entityStorePoint.LosePoint(_storePointCost, true);
        yield return new WaitForSeconds(_dash.Duration);
        _targetAnimator?.SetBool("BloodFist_Dash", false);
    }

    protected override void _OnAttackTrigger(Collider2D collider) {
        //Debug.Log(collider.transform.gameObject);
        //if (collider.CompareTag("Boss") || collider.CompareTag("Enemy")) {
        //    //Debug.Log("touché");
        //    _entityStorePoint.GainPoint(_hitStorePoint);
        //}
        //if (_entityStorePoint.CurrentValue <= 0f) { return; }
        if (_healedColliders.Contains(collider)) { return; }
        if (collider.CompareTag("Player") && collider.gameObject.GetRoot() != User.gameObject && !collider.gameObject.GetComponentInRoot<IHealth>().IsDead/*&& !collider.gameObject.GetRoot().transform.IsChildOf(User.gameObject.transform)*/) {
            collider.gameObject.GetComponentInRoot<IHealth>()?.TakeHeal(_healthPercentRegen);
            _healedColliders.Add(collider);
            //_entityStorePoint.LosePoint(_hitStorePointCost, false);
            //collider.gameObject.GetRoot().GetComponentInChildren<EntityMovement>()?.CreateMovement(_pushDuration, _pushStrenght, collider.gameObject.transform.position - transform.position, _pushCurve);
            //Vector2 bumpDirection = (collider.gameObject.transform.position - transform.position).normalized;
            //collider.gameObject.GetComponentInRoot<EntityAbilities>()?
            //    .Get<EntityPhysics>()?
            //    .Add(new Force(_bumpForce, _lastFistDirection, 0.7f, Force.ForceMode.TIMED), (int)PhysicPriority.PROJECTION);
        }
    }

    private void GainBloodPoint(IHealth health, int damage) {
        _entityStorePoint.GainPoint(_hitStorePoint);
    }
}
