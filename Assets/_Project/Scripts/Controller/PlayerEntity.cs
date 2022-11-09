using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class PlayerEntity : MonoBehaviour , IEntityAbility {
    [SerializeField] Transform _root;
    //[SerializeField] EntityMovement _movements;
    [SerializeField] EntityAbilities _abilities;
    [SerializeField] PhysicForce _dashForce;
    [Header("Abilities")]
    [SerializeField] EntityPhysicMovement _pMovements;
    [SerializeField] EntityOrientation _oriention;
    [SerializeField] EntityHolding _holding;
    [SerializeField] EntityWeaponry _weaponry;
    [SerializeField] EntityTryCatch _catch;
    [SerializeField] EntityDirectionnalSprite _directionnalSprite;
    [SerializeField] EntityInvincibility _invincibility;
    [SerializeField] ColliderDelegate _interactCollider;
    [Header("Graphics")]
    [SerializeField] Animator _animator;
    [SerializeField] TrajectoryLine _trajectoryLine;

    [SerializeField] BetterEvent<Vector2> _onAim = new BetterEvent<Vector2>();

    Token _canLookAround = new Token();
    Token _showAimLine = new Token();

    public bool HasWeapon => _weaponry.HasWeapon;
    public Weapon Weapon => _weaponry.Weapon;
    public Vector2 Orientation => _oriention.Orientation;
    public bool CanLookAround { get => !_canLookAround.HasToken; set => _canLookAround.AddToken(!value); }

    public event UnityAction<Vector2> OnAim { add => _onAim += value; remove => _onAim -= value; }

    private void OnEnable() {
        _interactCollider.OnCollisionEnter += _Pickup;
        _interactCollider.OnTriggerEnter += _Pickup;

        _dashForce.OnStart += _OnDash;
        _dashForce.OnEnd += _OnStopDash;

        _weaponry.OnAttack += _OnAttackStart;
        _weaponry.OnAttackEnd += _OnAttackEnd;
    }

    public void Move(Vector2 direction) {
        //if (!_movements.CanMove) { return; }
        if (!_pMovements.CanMove) { return; }
        //_movements.Move(direction);
        _pMovements.Move(direction);
        if (direction != Vector2.zero) { _directionnalSprite?.ChangeSprite(direction); }
        if (_animator == null) { return; }
        _animator?.SetBool("Running", direction != Vector2.zero);
        if (direction != Vector2.zero) { _animator?.SetFloat("x", direction.x); _animator?.SetFloat("y", direction.y); }
    }

    public void Aim(Vector2 direction, bool aiming = true) {
        if (!CanLookAround) { return; }
        //if (direction == Vector2.zero) { direction = _movements.Orientation; }
        _oriention.LookAt(direction);
        if (aiming) { _weaponry.Aim(direction); }
        if (_trajectoryLine != null) { _trajectoryLine.Direction = direction; }
        _onAim.Invoke(direction);
    }

    public void PressAttack(AttackIndex type, Vector2 direction) {
        if (_weaponry.HasWeapon) {
            _weaponry.PressAttack(type, _abilities, direction);
        }
    }

    public void PressAttackEnd(AttackIndex type) {
        if (_weaponry.HasWeapon) {
            _weaponry.PressAttackEnd(type, _abilities);
        }
    }

    public void Dash(Vector2 direction) {
        _dashForce.Use(direction, (int)PhysicPriority.DASH);
    }

    public void ShowAimLine(bool state) {
        _showAimLine.AddToken(state);
        _trajectoryLine?.gameObject.SetActive(_showAimLine.HasToken);
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

    public void TryCatch() {
        _catch.TryCatch();
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

    private void _OnDash() {
        CanLookAround = false;
        _invincibility.Invincible(true);
    }

    private void _OnStopDash() {
        CanLookAround = true;
        _invincibility.Invincible(false);
    }

    private void _OnAttackStart(Weapon weapon, AttackIndex type, Vector2 direction) {
        if (weapon.GetAttack(type).attackTime > 0f) {
            //_movements.Stop();
            _pMovements.Stop();
            //_movements.CanMove = false;
            _pMovements.CanMove = false;
            CanLookAround = false;
        }
    }

    private void _OnAttackEnd(Weapon weapon, AttackIndex type) {
        if (weapon.GetAttack(type).attackTime > 0f) {
            _pMovements.CanMove = true;
            CanLookAround = true;
        }
    }

    #endregion
}
