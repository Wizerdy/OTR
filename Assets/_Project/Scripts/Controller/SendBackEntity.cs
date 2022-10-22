using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class SendBackEntity : MonoBehaviour {
    [SerializeField] Transform _root;
    [SerializeField] ColliderDelegate _interactCollider;
    [SerializeField] EntityMovement _movements;
    [SerializeField] EntityOrientation _oriention;
    [SerializeField] EntityHolding _holding;
    [SerializeField] EntityWeaponry _weaponry;

    [SerializeField] BetterEvent<Vector2> _onAim = new BetterEvent<Vector2>();

    Token _canLookAround = new Token();

    public Vector2 Orientation => _oriention.Orientation;
    public bool CanLookAround { get => !_canLookAround.HasToken; set => _canLookAround.AddToken(!value); }

    public event UnityAction<Vector2> OnAim { add => _onAim += value; remove => _onAim -= value; }

    private void OnEnable() {
        _interactCollider.OnCollisionEnter += _Pickup;
        _interactCollider.OnTriggerEnter += _Pickup;
    }

    public void Aim(Vector2 direction) {
        if (!CanLookAround) { return; }
        _oriention.LookAt(direction);
        _weaponry.Aim(direction);
        _onAim.Invoke(direction);
    }

    public void Attack(Vector2 direction) {
        if (_weaponry.HasWeapon) {
            _weaponry.PressAttack(AttackIndex.FIRST, null, direction);
        } else {
            _movements.Dash(Orientation);
        }
    }

    #region Item interaction

    public void Pickup(GameObject obj) {
        _holding.Pickup(obj);
        Weapon weapon = obj.GetComponentInRoot<Weapon>();
        if (weapon != null) {
            _weaponry.Pickup(weapon);
            Throw(FindObjectOfType<PlayerEntity>().transform.position - transform.position);
        }
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
    #endregion
}
