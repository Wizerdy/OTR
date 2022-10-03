using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class EntityWeaponry : MonoBehaviour, IEntityAbility {
    [SerializeField] EntityMovement _entityMovement;
    [SerializeField] Animator _attackAnimator;
    [SerializeField] DamageHealth _damageHealth;
    [SerializeField] Health _health;

    Weapon _weapon;
    EntityMovement.SpeedModifier _movementSlow;

    [SerializeField, HideInInspector] BetterEvent<AttackIndex, Vector2> _onAttack = new BetterEvent<AttackIndex, Vector2>();
    [SerializeField, HideInInspector] BetterEvent<AttackIndex> _onAttackEnd = new BetterEvent<AttackIndex>();

    #region Properties

    public Weapon Weapon { get { return _weapon; } set { _weapon = value; } }
    public Animator Animator => _attackAnimator;
    public Health Health => _health;
    public bool HasWeapon => _weapon != null;

    public event UnityAction<AttackIndex, Vector2> OnAttack { add => _onAttack += value; remove => _onAttack -= value; }
    public event UnityAction<AttackIndex> OnAttackEnd { add => _onAttackEnd += value; remove => _onAttackEnd -= value; }

    #endregion

    private void Start() {
        _movementSlow = _entityMovement.Slow(1f, 0f);
    }

    private void Update() {
        _weapon?.PickedUpdate();
    }

    public void Pickup(Weapon weapon) {
        if (weapon == null) { return; }
        Drop();
        weapon.Pickup(this);
        weapon.OnMovespeedSet += SetMovementSlow;
        weapon.OnAttackStart += _InvokeOnAttack;
        weapon.OnAttackEnd += _InvokeOnAttackEnd;
        _weapon = weapon;
        _damageHealth.Damage = _weapon.Damage;
    }

    public void Drop() {
        if (_weapon == null) { return; }
        _weapon.Drop(this);
        _weapon.OnMovespeedSet -= SetMovementSlow;
        _weapon.OnAttackStart -= _InvokeOnAttack;
        _weapon.OnAttackEnd -= _InvokeOnAttackEnd;
        SetMovementSlow(1f);
        _weapon = null;
    }

    public void PressAttack(AttackIndex type, Vector2 direction) {
        if (!HasWeapon || !_weapon.CanAttack) { return; }
        _damageHealth.ResetHitted();
        StartCoroutine(_weapon.Attack(type, direction));
    }

    public void PressAttackEnd(AttackIndex type) {
        if (!HasWeapon || _weapon.CanAttack) { return; }
        _weapon.PressAttackEnd(type);
    }

    public void Aim(Vector2 direction) {
        if (_weapon == null) { return; }

        _weapon.Aim(direction);
    }

    public void SetMovementSlow(float slow) {
        _entityMovement.SetSlow(_movementSlow, slow);
    }

    private void _InvokeOnAttack(AttackIndex type, Vector2 direction) {
        _onAttack.Invoke(type, direction);
    }

    private void _InvokeOnAttackEnd(AttackIndex type) {
        _onAttackEnd.Invoke(type);
    }
}
