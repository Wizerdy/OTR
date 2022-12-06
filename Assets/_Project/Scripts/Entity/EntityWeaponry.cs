using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class EntityWeaponry : MonoBehaviour, IEntityAbility {
    [SerializeField] EntityAbilities _entityAbilities;
    [SerializeField] EntityMovement _entityMovement;
    [SerializeField] Animator _attackAnimator;
    [SerializeField] DamageHealth _damageHealth;
    [SerializeField] Health _health;

    Weapon _weapon;
    EntityMovement.SpeedModifier _movementSlow;

    [SerializeField] BetterEvent<Weapon> _onPickup = new BetterEvent<Weapon>();
    [SerializeField, HideInInspector] BetterEvent<Weapon> _onDrop = new BetterEvent<Weapon>();
    [SerializeField, HideInInspector] BetterEvent<Weapon, AttackIndex, Vector2> _onAttack = new BetterEvent<Weapon, AttackIndex, Vector2>();
    [SerializeField, HideInInspector] BetterEvent<Weapon, AttackIndex> _onAttackEnd = new BetterEvent<Weapon, AttackIndex>();
    [SerializeField, HideInInspector] BetterEvent<Collider2D> _onAttackTrigger = new BetterEvent<Collider2D>();
    [SerializeField, HideInInspector] BetterEvent<Weapon, AttackIndex, IHealth, int> _onAttackHit = new BetterEvent<Weapon, AttackIndex, IHealth, int>();

    #region Properties

    public Weapon Weapon { get { return _weapon; } set { _weapon = value; } }
    public Animator Animator => _attackAnimator;
    public Health Health => _health;
    public DamageHealth DamageHealth => _damageHealth;
    public bool HasWeapon => _weapon != null;

    public event UnityAction<Weapon> OnPickup { add => _onPickup += value; remove => _onPickup -= value; }
    public event UnityAction<Weapon> OnDrop { add => _onDrop += value; remove => _onDrop -= value; }
    public event UnityAction<Weapon, AttackIndex, Vector2> OnAttack { add => _onAttack += value; remove => _onAttack -= value; }
    public event UnityAction<Weapon, AttackIndex> OnAttackEnd { add => _onAttackEnd += value; remove => _onAttackEnd -= value; }
    public event UnityAction<Collider2D> OnAttackTrigger { add => _onAttackTrigger += value; remove => _onAttackTrigger -= value; }
    public event UnityAction<Weapon, AttackIndex, IHealth, int> OnAttackHit { add => _onAttackHit += value; remove => _onAttackHit -= value; }

    #endregion

    private void Start() {
        //_movementSlow = _entityMovement.Slow(1f, 0f);
    }

    private void Update() {
        _weapon?.PickedUpdate();
    }

    public void Pickup(Weapon weapon) {
        if (weapon == null) { return; }
        Drop();
        weapon.Pickup(this, _entityAbilities);
        weapon.OnMovespeedSet += SetMovementSlow;
        weapon.OnAttackStart += _InvokeOnAttack;
        weapon.OnAttackEnd += _InvokeOnAttackEnd;
        weapon.OnAttackTrigger += _InvokeOnAttackTrigger;
        weapon.OnAttackHit += _InvokeOnAttackHit;
        _weapon = weapon;
        _onPickup.Invoke(weapon);
    }

    public void Drop() {
        if (_weapon == null) { return; }
        _weapon.Drop(this);
        _weapon.OnMovespeedSet -= SetMovementSlow;
        _weapon.OnAttackStart -= _InvokeOnAttack;
        _weapon.OnAttackEnd -= _InvokeOnAttackEnd;
        _weapon.OnAttackTrigger -= _InvokeOnAttackTrigger;
        SetMovementSlow(1f);
        _onDrop.Invoke(_weapon);
        _weapon = null;
    }

    public void PressAttack(AttackIndex type, EntityAbilities caster, Vector2 direction) {
        if (!HasWeapon || !_weapon.CanAttack) { return; }
        _damageHealth.ResetHitted();
        StartCoroutine(_weapon.Attack(type, caster, direction));
    }

    public void PressAttackEnd(AttackIndex type, EntityAbilities caster) {
        if (!HasWeapon || _weapon.CanAttack) { return; }
        _weapon.PressAttackEnd(type, caster);
    }

    public void Aim(Vector2 direction) {
        if (_weapon == null) { return; }

        _weapon.Aim(direction);
    }

    public void SetMovementSlow(float slow) {
        //_entityMovement.SetSlow(_movementSlow, slow);
    }

    private void _InvokeOnAttack(AttackIndex type, Vector2 direction) {
        _onAttack.Invoke(_weapon, type, direction);
    }

    private void _InvokeOnAttackEnd(AttackIndex type) {
        _onAttackEnd.Invoke(_weapon, type);
    }

    private void _InvokeOnAttackTrigger(Collider2D collider) {
        _onAttackTrigger.Invoke(collider);
    }

    private void _InvokeOnAttackHit(AttackIndex index, IHealth health, int damage) {
        _onAttackHit.Invoke(_weapon, index, health, damage);
    }
}
