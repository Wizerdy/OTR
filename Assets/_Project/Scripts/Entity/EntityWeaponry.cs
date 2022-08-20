using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWeaponry : MonoBehaviour {
    [SerializeField] EntityMovement _entityMovement;
    [SerializeField] Animator _attackAnimator;
    [SerializeField] DamageHealth _damageHealth;
    [SerializeField] Health _health;
    Weapon _weapon;
    EntityMovement.SpeedModifier _movementSlow;

    #region Properties

    public Weapon Weapon { get { return _weapon; } set { _weapon = value; } }
    public Animator Animator => _attackAnimator;
    public Health Health => _health;
    public bool HasWeapon => _weapon != null;

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
        _weapon = weapon;
        _damageHealth.Damage = _weapon.Damage;
    }

    public void Drop() {
        if (_weapon == null) { return; }
        _weapon.Drop(this);
        _weapon.OnMovespeedSet -= SetMovementSlow;
        SetMovementSlow(1f);
        _weapon = null;
    }

    public void Attack(Vector2 direction) {
        if (!HasWeapon || !_weapon.CanAttack) { return; }
        _damageHealth.ResetHitted();
        StartCoroutine(_weapon.Attack(direction));
    }

    public void Aim(Vector2 direction) {
        if (_weapon == null) { return; }

        _weapon.Aim(direction);
    }

    public void SetMovementSlow(float slow) {
        _entityMovement.SetSlow(_movementSlow, slow);
    }
}
