using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

[System.Serializable]
public class DamageHealth : MonoBehaviour {
    [SerializeField] GameObject _root = null;
    [SerializeField] int _damage = 5;
    [SerializeField] MultipleTagSelector _damageables;
    [SerializeField] bool _destroyOnHit;
    [SerializeField] bool _onlyDamageOnceEach = false;
    [SerializeField] string _damageType;
    [SerializeField] MultipleTagSelector _ignoreTag;

    [SerializeField, HideInInspector] BetterEvent<Collision2D> _onCollide = new BetterEvent<Collision2D>();
    [SerializeField, HideInInspector] BetterEvent<Collider2D> _onTrigger = new BetterEvent<Collider2D>();
    [SerializeField, HideInInspector] BetterEvent<IHealth, int> _onDamage = new BetterEvent<IHealth, int>();

    List<GameObject> _hitted = new List<GameObject>();

    #region Properties

    public int Damage { get { return _damage; } set { _damage = value; } }
    public string DamageType { get { return _damageType; } set { _damageType = value; } }
    public MultipleTagSelector Damageables { get { return _damageables; } set { _damageables = value; } }

    public event UnityAction<Collision2D> OnCollide { add { _onCollide.AddListener(value); } remove { _onCollide.RemoveListener(value); } }
    public event UnityAction<Collider2D> OnTrigger { add { _onTrigger.AddListener(value); } remove { _onTrigger.RemoveListener(value); } }
    public event UnityAction<IHealth, int> OnDamage { add { _onDamage.AddListener(value); } remove { _onDamage.RemoveListener(value); } }

    #endregion

    private void OnCollisionEnter2D(Collision2D collision) {
        Collision(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        Trigger(collision);
    }

    public void Collision(Collision2D collision) {
        GameObject obj = collision.gameObject;
        bool hardHit = !collision.collider.isTrigger;
        if (_ignoreTag.Contains(obj.tag)) { return; }

        _onCollide?.Invoke(collision);

        Collide(obj, hardHit);
    }

    public void Trigger(Collider2D collider) {
        GameObject obj = collider.gameObject;
        bool hardHit = !collider.isTrigger;
        if (_ignoreTag.Contains(obj.tag)) { return; }

        _onTrigger?.Invoke(collider);

        Collide(obj, hardHit);
    }

    private void Collide(GameObject obj, bool hardHit) {
        if (_damageables.Contains(obj.tag)) {
            GameObject root = obj.GetComponent<ColliderRoot>()?.Root ?? obj;
            if (_onlyDamageOnceEach && _hitted.Contains(root)) { return; }
            _hitted.Add(root);
            IHealth health = obj.GetComponent<IHealth>();
            if (health != null && health.CanTakeDamage) {
                health.TakeDamage(_damage, _damageType);
                _onDamage?.Invoke(health, _damage);
            }
            if (_destroyOnHit) {
                Die();
            }
            return;
        }

        if (hardHit && _destroyOnHit) {
            Die();
        }
    }

    public void ResetHitted() {
        _hitted.Clear();
    }

    public void Die() {
        if (_root != null) {
            _root.gameObject.SetActive(false);
            Destroy(_root);
        } else {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void SetValues(MultipleTagSelector damageables, int damage) {
        if (damageables != null) {
            _damageables = damageables;
        }
        _damage = damage;
        ResetHitted();
    }

    public void SetDamage(int damage) {
        _damage = damage;
    }

    public void NotEditable() {
        this.hideFlags = HideFlags.NotEditable;
    }

    public void Hit(Collider2D collision) {
        Collide(collision.gameObject, !collision.isTrigger);
    }
}

