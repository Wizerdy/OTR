using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class PlayerEntity : MonoBehaviour {
    [SerializeField] Transform _root;
    [SerializeField] EntityAbilities _abilities;
    [SerializeField] EntityMovement _movements;
    [SerializeField] EntityOrientation _oriention;
    [SerializeField] EntityHolding _holding;
    [SerializeField] EntityWeaponry _weaponry;
    [SerializeField] EntityDirectionnalSprite _directionnalSprite;
    [SerializeField] ColliderDelegate _interactCollider;

    [SerializeField] BetterEvent<Vector2> _onAim = new BetterEvent<Vector2>();

    Token _canLookAround = new Token();

    public bool HasWeapon => _weaponry.HasWeapon;
    public Vector2 Orientation => _oriention.Orientation;
    public bool CanLookAround { get => !_canLookAround.HasToken; set => _canLookAround.AddToken(!value); }

    public event UnityAction<Vector2> OnAim { add => _onAim += value; remove => _onAim -= value; }

    private void OnEnable() {
        _interactCollider.OnCollisionEnter += _Pickup;
        _interactCollider.OnTriggerEnter += _Pickup;

        _movements.OnDashStart += _OnDash;
        _movements.OnDashEnd += _OnStopDash;

        _weaponry.OnAttack += _OnAttackStart;
        _weaponry.OnAttackEnd += _OnAttackEnd;
    }

    public void Move(Vector2 direction) {
        if (!_movements.CanMove) { return; }
        _movements.Move(direction);
        if (direction != Vector2.zero) { _directionnalSprite?.ChangeSprite(direction); }
    }

    public void Aim(Vector2 direction) {
        if (!CanLookAround) { return; }
        //if (direction == Vector2.zero) { direction = _movements.Orientation; }
        _oriention.LookAt(direction);
        _weaponry.Aim(direction);
        _onAim.Invoke(direction);
    }

    public void PressAttack(AttackIndex type, Vector2 direction) {
        if (_weaponry.HasWeapon) {
            _weaponry.PressAttack(type, direction);
        }
    }

    public void PressAttackEnd(AttackIndex type) {
        if (_weaponry.HasWeapon) {
            _weaponry.PressAttackEnd(type);
        }
    }

    public void Dash(Vector2 direction) {
        _movements.Dash(direction);
    }

    #region Item interaction

    public void Pickup(GameObject obj) {
        _holding.Pickup(obj);
        Weapon weapon = obj.GetComponentInRoot<Weapon>();
        if (weapon != null) {
            _weaponry.Pickup(weapon);
        }
    }

    public void Drop() {
        _holding.Drop();
        _weaponry.Drop();
    }

    public void Throw(Vector2 direction) {
        _holding.Throw(direction, _root.gameObject);
        _weaponry.Drop();
    }

    #endregion

    #region Callbacks

    private void _Pickup(Collision2D collision) {
        _Pickup(collision.collider);
    }

    private void _Pickup(Collider2D collision) {
        if (_holding.IsHolding) { return; }
        GameObject root;
        if (collision.gameObject.GetComponentInRoot<IHoldable>(out root) != null) {
            Pickup(root);
        }
    }

    private void _OnDash(Vector2 direction) {
        CanLookAround = false;

    }

    private void _OnStopDash() {
        CanLookAround = true;
    }

    private void _OnAttackStart(AttackIndex type, Vector2 direction) {
        _movements.Stop();
        _movements.CanMove = false;
        CanLookAround = false;
    }

    private void _OnAttackEnd(AttackIndex type) {
        _movements.CanMove = true;
        CanLookAround = true;
    }

    #endregion
}
