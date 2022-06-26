using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHolding : MonoBehaviour {
    [SerializeField] Animator _animator;
    [SerializeField] Weapon _weapon;

    #region Properties

    public Weapon Weapon { get { return _weapon; } set { _weapon = value; } }
    public Animator Animator => _animator;
    public bool HasWeapon => _weapon != null;

    #endregion

    private void Start() {
        if (_weapon != null) {
            _weapon.Pickup(this);
        }
    }

    public void Pickup(Weapon weapon) {
        if (weapon == _weapon) { return; }
        Drop();
        weapon.Pickup(this);
    }

    public void Drop() {
        if (!HasWeapon) { return; }
        _weapon.Drop(transform.position);
        _weapon = null;
    }

    public void Attack(Vector2 direction) {
        if (!HasWeapon || !_weapon.CanAttack) { return; }
        StartCoroutine(_weapon.Attack(direction));
    }

    public void Throw(Vector2 direction, Collider2D collider = null) {
        if (!HasWeapon) { return; }
        Weapon weapon = _weapon;
        Drop();
        weapon.Throw(direction, collider);
    }
}
