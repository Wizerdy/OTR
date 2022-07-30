using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityWeaponry : MonoBehaviour {
    [SerializeField] Animator _attackAnimator;
    [SerializeField] DamageHealth _damageHealth;
    [SerializeField] Health _health;
    Weapon _weapon;

    #region Properties

    public Weapon Weapon { get { return _weapon; } set { _weapon = value; } }
    public Animator Animator => _attackAnimator;
    public Health Health => _health;
    public bool HasWeapon => _weapon != null;

    #endregion

    private void Update() {
        _weapon?.PickedUpdate();
    }

    public void Pickup(Weapon weapon) {
        if (weapon == null) { return; }
        Drop();
        weapon.Pickup(this);
        _weapon = weapon;
        _damageHealth.Damage = _weapon.Damage;
    }

    public void Drop() {
        if (_weapon == null) { return; }
        _weapon.Drop(this);
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
}
