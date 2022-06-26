using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntity : MonoBehaviour {
    [SerializeField] ColliderDelegate _colliderDelegate;
    [SerializeField] Collider2D _collider;
    [SerializeField] EntityMovement _movements;
    [SerializeField] EntityOrientation _oriention;
    [SerializeField] EntityHolding _holding;

    public Vector2 Orientation => _oriention.Orientation;

    private void OnEnable() {
        _colliderDelegate.OnCollisionEnter += _Pickup;
    }

    public void Move(Vector2 direction) {
        _movements.Move(direction);
    }

    public void LookAt(Vector2 direction) {
        _oriention.LookAt(direction);
    }

    public void Pickup(Weapon weapon) {
        _holding.Pickup(weapon);
    }

    public void Drop() {
        _holding.Drop();
    }

    public void Attack(Vector2 direction) {
        _holding.Attack(direction);
    }

    public void Throw(Vector2 direction) {
        _holding.Throw(direction, _collider);
    }

    private void _Pickup(Collision2D collision) {
        Weapon weapon = collision.gameObject.GetComponent<Weapon>();
        if (weapon != null) {
            Pickup(weapon);
        }
    }
}
