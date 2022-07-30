using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public abstract class Weapon : MonoBehaviour, IHoldable {
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected int _damage = 10;
    [SerializeField] protected float _throwPower = 50f;

    [SerializeField] protected BetterEvent _onAttackEnd = new BetterEvent();

    protected Animator _targetAnimator;

    protected Collider2D[] _colliders;

    bool _canAttack = true;
    bool _attacking = false;

    #region Properties

    public event UnityAction OnAttackEnd { add => _onAttackEnd.AddListener(value); remove => _onAttackEnd.RemoveListener(value); }
    public int Damage => _damage;
    public bool IsAttacking => _attacking;
    public bool CanAttack => !IsAttacking && _canAttack;

    #endregion

    #region Legacy

    protected virtual void _OnPickedUpdate() { }
    protected virtual void _OnAim(Vector2 direction) { }
    protected virtual void _OnPickup(EntityHolding holding) { }
    protected virtual void _OnPickup(EntityWeaponry weaponry) { }
    protected virtual void _OnDrop(EntityHolding holding) { }
    protected virtual void _OnDrop(EntityWeaponry weaponry) { }
    protected virtual void _OnThrow(EntityHolding holding, Vector2 direction, Collider2D collider = null) { }
    protected virtual void _OnThrow(EntityHolding holding, Vector2 direction, GameObject obj) { }

    #endregion

    #region Unity Callbacks

    private void Awake() {
        _colliders = GetComponentsInChildren<Collider2D>();
    }

    public void PickedUpdate() {
        _OnPickedUpdate();
    }

    #endregion

    public void Aim(Vector2 direction) {
        _OnAim(direction);
    }

    public IEnumerator Attack(Vector2 direction) {
        if (!CanAttack) { yield return null; }
        _attacking = true;
        yield return IAttack(direction);
        _attacking = false;
        _onAttackEnd.Invoke();
    }

    //IEnumerator ILaunchAttack(Vector2 direction) { }

    protected abstract IEnumerator IAttack(Vector2 direction);

    #region IHoldable

    public void Drop(EntityHolding holding, Vector2 position) {
        _OnDrop(holding);
        transform.parent = null;
        _targetAnimator = null;
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Pickup(EntityHolding holding) {
        transform.parent = holding.transform;
        gameObject.SetActive(false);
        _OnPickup(holding);
    }

    public void Drop(EntityWeaponry weaponry) {
        _OnDrop(weaponry);
        _targetAnimator = null;
    }

    public void Pickup(EntityWeaponry weaponry) {
        _targetAnimator = weaponry.Animator;
        _OnPickup(weaponry);
    }

    public void Throw(EntityHolding entityHolding, Vector2 direction, Collider2D collider = null) {
        direction.Normalize();
        _OnThrow(entityHolding, direction, collider);
        _rb.velocity = direction * _throwPower;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        IgnoreCollider(collider, true, 0.5f);
    }

    public void Throw(EntityHolding entityHolding, Vector2 direction, GameObject obj) {
        direction.Normalize();
        _OnThrow(entityHolding, direction, obj);
        _rb.velocity = direction * _throwPower;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        IgnoreCollider(obj, true, 0.5f);
    }

    #endregion

    #region IgnoreCollider

    public void IgnoreCollider(GameObject obj, bool state, float time) {
        Collider2D[] colliders = obj.GetComponentsInChildren<Collider2D>();

        for (int i = 0; i < colliders.Length; i++) {
            IgnoreCollider(colliders[i], state, time);
        }
    }

    public void IgnoreCollider(Collider2D collider, bool state = true) {
        if (collider == null || _colliders == null || _colliders.Length <= 0) { return; }

        for (int i = 0; i < _colliders.Length; i++) {
            Physics2D.IgnoreCollision(_colliders[i], collider, state);
        }
    }

    public void IgnoreCollider(Collider2D collider, bool state, float time) {
        if (collider == null || _colliders == null || _colliders.Length <= 0) { return; }

        IgnoreCollider(collider, state);

        if (time > 0f) {
            StartCoroutine(Tools.Delay(IgnoreCollider, collider, !state, time));
        }
    }

    #endregion
}
