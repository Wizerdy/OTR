using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public abstract class Weapon : MonoBehaviour, IHoldable, IReflectable {
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected bool _isOnFloor = true;
    [SerializeField] protected bool _isInAir = true;
    [SerializeField] protected int _damage = 10;
    [SerializeField] protected float _throwPower = 50f;
    [SerializeField, Range(0f, 1f)] private float _movespeed = 1f;

    [SerializeField] protected BetterEvent<Vector2> _onAttackStart = new BetterEvent<Vector2>();
    [SerializeField] protected BetterEvent _onAttackEnd = new BetterEvent();
    [SerializeField] protected BetterEvent _onFall = new BetterEvent();
    [SerializeField, HideInInspector] protected BetterEvent<float> _onMovespeedSet = new BetterEvent<float>();

    protected Animator _targetAnimator;

    protected Collider2D[] _colliders;

    protected bool _canAttack = true;
    protected bool _attacking = false;

    protected Vector2 _lastVelocity = Vector2.zero;

    #region Properties

    public event UnityAction<Vector2> OnAttackStart { add => _onAttackStart += value; remove => _onAttackStart -= value; }
    public event UnityAction OnAttackEnd { add => _onAttackEnd.AddListener(value); remove => _onAttackEnd.RemoveListener(value); }
    public event UnityAction OnFall { add => _onFall.AddListener(value); remove => _onFall.RemoveListener(value); }
    public event UnityAction<float> OnMovespeedSet { add => _onMovespeedSet.AddListener(value); remove => _onMovespeedSet.RemoveListener(value); }
    public int Damage => _damage;
    public bool IsAttacking => _attacking;
    public bool CanAttack => !IsAttacking && _canAttack;
    public bool IsInAir => _isInAir;
    protected float MoveSpeed { get => _movespeed; set { _movespeed = value; _onMovespeedSet.Invoke(value); } }

    #endregion

    #region Legacy

    protected virtual void _OnStart() { }
    protected virtual void _OnAttackEnd() { }
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

    private void Reset() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Awake() {
        _colliders = GetComponentsInChildren<Collider2D>();
    }

    private void Start() {
        FallOnFloor(_isOnFloor);
        _OnStart();
    }

    public void PickedUpdate() {
        _OnPickedUpdate();
    }

    private void Update() {
        _lastVelocity = _rb?.velocity ?? Vector2.zero;
    }

    #endregion

    public void Aim(Vector2 direction) {
        _OnAim(direction);
    }

    public IEnumerator Attack(Vector2 direction) {
        if (!CanAttack) { yield return null; }
        _attacking = true;
        _onAttackStart.Invoke(direction);
        yield return IAttack(direction);
        _attacking = false;
        _onAttackEnd.Invoke();
    }

    public void AttackEnd() {
        _OnAttackEnd();
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
        transform.localPosition = Vector2.zero;
        gameObject.SetActive(false);
        _OnPickup(holding);
    }

    public void Drop(EntityWeaponry weaponry) {
        _OnDrop(weaponry);
        _targetAnimator = null;
    }

    public void Pickup(EntityWeaponry weaponry) {
        _targetAnimator = weaponry.Animator;
        weaponry.SetMovementSlow(_movespeed);
        _OnPickup(weaponry);
    }

    public void Throw(EntityHolding entityHolding, Vector2 direction, Collider2D collider = null) {
        if (_isOnFloor) { FallOnFloor(false); }
        direction.Normalize();
        _OnThrow(entityHolding, direction, collider);
        _rb.velocity = direction * _throwPower;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        IgnoreCollider(collider, true, 0.5f);
        StartCoroutine(CheckFalling());
    }

    public void Throw(EntityHolding entityHolding, Vector2 direction, GameObject obj) {
        if (_isOnFloor) { FallOnFloor(false); }
        direction.Normalize();
        _OnThrow(entityHolding, direction, obj);
        _rb.velocity = direction * _throwPower;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        IgnoreCollider(obj, true, 0.5f);
        StartCoroutine(CheckFalling());
    }

    private IEnumerator CheckFalling() {
        if (_rb == null) { yield break; }
        while (_rb.velocity.sqrMagnitude > 0.1f) {
            yield return null;
        }
        _rb.velocity = Vector2.zero;
        FallOnFloor(true);
        _onFall.Invoke();
    }

    public void FallOnFloor(bool state = true) {
        for (int i = 0; i < _colliders.Length; i++) {
            _colliders[i].isTrigger = state;
        }
        _isOnFloor = state;
    }

    #endregion

    #region IReflectable

    public void Launch(float force, Vector2 direction) {
        if (direction == Vector2.zero) { return; }
        direction.Normalize();
        transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
        _rb.velocity = direction * force;
    }

    public void Reflect(ContactPoint2D collision) {
        if (_lastVelocity.sqrMagnitude <= 0.5f) { return; }
        Vector2 reflection = Vector2.Reflect(_lastVelocity, collision.normal);
        _rb.velocity = reflection;
        transform.rotation = Quaternion.LookRotation(Vector3.forward, _rb.velocity);
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
