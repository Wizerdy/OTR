using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour {
    [SerializeField] Transform _root;
    [SerializeField] ColliderDelegate _interactCollider;
    [SerializeField] EntityMovement _movements;
    [SerializeField] EntityOrientation _oriention;
    [SerializeField] EntityHolding _holding;
    [SerializeField] EntityWeaponry _weaponry;

    public Vector2 Orientation => _oriention.Orientation;

    private void OnEnable() {
        _interactCollider.OnCollisionEnter += _Pickup;
        _interactCollider.OnTriggerEnter += _Pickup;
    }

    public void Move(Vector2 direction) {
        _movements.Move(direction);
    }

    public void LookAt(Vector2 direction) {
        _oriention.LookAt(direction);
    }

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

    public void Attack(Vector2 direction) {
        _weaponry.Attack(direction);
    }

    public void Throw(Vector2 direction) {
        _holding.Throw(direction, _root.gameObject);
        _weaponry.Drop();
    }

    private void _Pickup(Collision2D collision) {
        _Pickup(collision.collider);
    }

    private void _Pickup(Collider2D collision) {
        GameObject root;
        if (collision.gameObject.GetComponentInRoot<IHoldable>(out root) != null) {
            Pickup(root);
        }
    }
}
