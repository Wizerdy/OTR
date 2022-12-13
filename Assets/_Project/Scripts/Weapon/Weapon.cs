using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public enum AttackIndex { FIRST, SECOND }
public enum WeaponType { UNDEFINED, SWORD, SHIELD, BOW, BLOODFIST, CROSSGUN, SHIELDAXE }

public class WeaponAttack {
    public float attackTime;
    public int damage;
    public int threatPoint;
    public System.Func<EntityAbilities, Vector2, IEnumerator> attack;

    public bool canAttack;

    public WeaponAttack(float attackTime, int damage, int threatPoint, System.Func<EntityAbilities, Vector2, IEnumerator> attack) {
        this.attackTime = attackTime;
        this.damage = damage;
        this.threatPoint = threatPoint;
        this.attack = attack;
        this.canAttack = true;
    }
}

public abstract class Weapon : MonoBehaviour, IHoldable, IReflectable {
    [SerializeField] protected Rigidbody2D _rb;
    [SerializeField] protected bool _isOnFloor = true;
    //[SerializeField] protected int _damage = 10;
    [SerializeField] protected float _throwPower = 50f;
    [SerializeField, Range(0f, 1f)] private float _movespeed = 1f;
    [SerializeField] bool _showAim = false;
    [SerializeField] float _minVelocityForOnFall;

    [SerializeField] protected BetterEvent<AttackIndex, Vector2> _onAttack = new BetterEvent<AttackIndex, Vector2>();
    [SerializeField] protected BetterEvent<AttackIndex> _onAttackEnd = new BetterEvent<AttackIndex>();
    [SerializeField] protected BetterEvent<Collider2D> _onAttackTrigger = new BetterEvent<Collider2D>();
    [SerializeField] protected BetterEvent<AttackIndex, IHealth, int> _onAttackHit = new BetterEvent<AttackIndex, IHealth, int>();
    [SerializeField] protected BetterEvent _onFall = new BetterEvent();
    [SerializeField] protected BetterEvent _onPickUp = new BetterEvent();
    [SerializeField, HideInInspector] protected BetterEvent<float> _onMovespeedSet = new BetterEvent<float>();

    protected Dictionary<AttackIndex, WeaponAttack> _attacks;

    protected Animator _targetAnimator;
    protected EntityWeaponry _weaponry;

    protected Collider2D[] _colliders;

    EntityAbilities _user = null;
    protected bool _canAttack = true;
    protected bool _attacking = false;
    protected WeaponType _type = WeaponType.UNDEFINED;

    protected Vector2 _lastVelocity = Vector2.zero;

    AttackIndex _currentIndex;

    #region Properties

    public event UnityAction<AttackIndex, Vector2> OnAttackStart { add => _onAttack += value; remove => _onAttack -= value; }
    public event UnityAction<AttackIndex> OnAttackEnd { add => _onAttackEnd.AddListener(value); remove => _onAttackEnd.RemoveListener(value); }
    public event UnityAction<Collider2D> OnAttackTrigger { add => _onAttackTrigger.AddListener(value); remove => _onAttackTrigger.RemoveListener(value); }
    public event UnityAction<AttackIndex, IHealth, int> OnAttackHit { add => _onAttackHit.AddListener(value); remove => _onAttackHit.RemoveListener(value); }

    public event UnityAction OnFall { add => _onFall.AddListener(value); remove => _onFall.RemoveListener(value); }
    public event UnityAction OnPickUp { add => _onPickUp.AddListener(value); remove => _onPickUp.RemoveListener(value); }
    public event UnityAction<float> OnMovespeedSet { add => _onMovespeedSet.AddListener(value); remove => _onMovespeedSet.RemoveListener(value); }

    public bool IsAttacking => _attacking;
    public bool CanAttack => !IsAttacking && _canAttack;
    public bool IsOnFloor => _isOnFloor;
    public WeaponType Type => _type;
    protected float MoveSpeed { get => _movespeed; set { _movespeed = value; _onMovespeedSet.Invoke(value); } }
    protected EntityAbilities User => _user;
    public bool ShowAim => _showAim;

    #endregion

    #region Legacy

    protected virtual void _OnStart() { }
    protected virtual void _OnPressAttackEnd(AttackIndex type, EntityAbilities caster) { }
    protected virtual void _OnPickedUpdate() { }
    protected virtual void _OnAim(Vector2 direction) { }
    protected virtual void _OnPickup(EntityHolding holding) { }
    protected virtual void _OnPickup(EntityWeaponry weaponry) { }
    protected virtual void _OnDrop(EntityHolding holding) { }
    protected virtual void _OnDrop(EntityWeaponry weaponry) { }
    protected virtual void _OnThrow(EntityHolding holding, Vector2 direction, Collider2D collider = null) { }
    protected virtual void _OnThrow(EntityHolding holding, Vector2 direction, GameObject obj) { }
    protected virtual void _OnAttackTrigger(Collider2D collider) { }
    protected virtual void _OnAttackHit(AttackIndex index, IHealth health, int damage) { }

    #endregion

    #region Unity Callbacks

    private void Reset() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Awake() {
        _colliders = GetComponentsInChildren<Collider2D>();
        _attacks = new Dictionary<AttackIndex, WeaponAttack>();
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

    //public abstract float AttackTime(AttackIndex index);

    public void Aim(Vector2 direction) {
        _OnAim(direction);
    }

    public IEnumerator Attack(AttackIndex type, EntityAbilities caster, Vector2 direction) {
        //if (!CanAttack) { yield break; }
        if (!_attacks.ContainsKey(type)) { yield break; }
        if (!_attacks[type].canAttack) { yield break; }
        _attacking = true;
        _onAttack.Invoke(type, direction);
        _currentIndex = type;
        _weaponry.DamageHealth.ResetHitted();
        _weaponry.DamageHealth.Damage = _attacks[type].damage;
        Debug.Log(_attacks[type].damage + " .. " + type);
        yield return _attacks[type].attack(caster, direction);
        _attacking = false;
        _onAttackEnd.Invoke(type);
    }

    public void PressAttackEnd(AttackIndex type, EntityAbilities caster) {
        _OnPressAttackEnd(type, caster);
    }

    public WeaponAttack GetAttack(AttackIndex index) {
        return _attacks[index];
    }

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
        _onPickUp.Invoke();
        _OnPickup(holding);
    }

    public void Drop(EntityWeaponry weaponry) {
        _OnDrop(weaponry);
        _weaponry = null;
        weaponry.DamageHealth.OnTrigger -= _InvokeAttackTrigger;
        weaponry.DamageHealth.OnDamage -= _InvokeAttackHit;
        _targetAnimator = null;
        _user = null;
    }

    public void Pickup(EntityWeaponry weaponry, EntityAbilities user = null) {
        _weaponry = weaponry;
        _targetAnimator = weaponry.Animator;
        weaponry.SetMovementSlow(_movespeed);
        weaponry.DamageHealth.OnTrigger += _InvokeAttackTrigger;
        weaponry.DamageHealth.OnDamage += _InvokeAttackHit;
        _user = user;
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
        while (_rb.velocity.sqrMagnitude > _minVelocityForOnFall * _minVelocityForOnFall) {
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

    protected void _InvokeAttackTrigger(Collider2D collider) {
        _OnAttackTrigger(collider);
        _onAttackTrigger.Invoke(collider);
    }

    protected void _InvokeAttackHit(IHealth health, int damage) {
        _OnAttackHit(_currentIndex, health, damage);
        _onAttackHit.Invoke(_currentIndex, health, damage);
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
