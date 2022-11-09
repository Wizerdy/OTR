using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;
using System.Linq;

public class EntityCollisionArea : MonoBehaviour {
    [SerializeField] ColliderDelegate _collider;
    [SerializeField] ColliderGate _colliderGate = ColliderGate.TRIGGER_HARD;

    [SerializeField] BetterEvent<GameObject> _onAreaEnter = new BetterEvent<GameObject>();
    [SerializeField] BetterEvent<GameObject> _onAreaExit = new BetterEvent<GameObject>();

    Dictionary<GameObject, Token> _objectsInside = new Dictionary<GameObject, Token>();

    public event UnityAction<GameObject> OnAreaEnter { add => _onAreaEnter.AddListener(value); remove => _onAreaEnter.RemoveListener(value); }
    public event UnityAction<GameObject> OnAreaExit { add => _onAreaExit.AddListener(value); remove => _onAreaExit.RemoveListener(value); }

    private void Start() {
        if (_collider == null) { Debug.LogWarning("No collider attached (field missing) : " + gameObject.name); return; }
        _collider.OnCollisionEnter += _OnCollisionEnter;
        _collider.OnTriggerEnter += _OnTriggerEnter;
        _collider.OnCollisionExit += _OnCollisionExit;
        _collider.OnTriggerExit += _OnTriggerExit;
    }

    private void EnterTheArea(GameObject obj) {
        GameObject objectRoot = obj.gameObject.GetRoot();
        if (!_objectsInside.ContainsKey(objectRoot)) {
            _objectsInside.Add(objectRoot, new Token(1));
            _onAreaEnter.Invoke(objectRoot);
        } else {
            _objectsInside[objectRoot].AddToken(1);
        }
    }

    private void LeaveTheArea(GameObject obj) {
        GameObject objectRoot = obj.gameObject.GetRoot();
        if (!_objectsInside.ContainsKey(objectRoot)) {
            Debug.LogWarning("Not entered ? o.o " + objectRoot.name);
            return;
        } else {
            _objectsInside[objectRoot].AddToken(-1);
            if (!_objectsInside[objectRoot].HasToken) {
                _objectsInside.Remove(objectRoot);
                _onAreaExit.Invoke(objectRoot);
            }
        }
    }
    public bool IsEmpty() {
        return _objectsInside.Count == 0;
    }

    public bool IsInArea(GameObject obj) {
        return _objectsInside.ContainsKey(obj.GetRoot());
    }

    public GameObject Nearest() {
        if (_objectsInside.Count <= 0) { return null; }
        float nearestDistance = Mathf.Infinity;
        GameObject nearest = null;
        foreach (KeyValuePair<GameObject, Token> obj in _objectsInside) {
            float distance = Vector2.Distance(transform.Position2D(), obj.Key.transform.Position2D());
            if (distance < nearestDistance) {
                nearest = obj.Key;
                distance = nearestDistance;
            }
        }
        return nearest;
    }

    public GameObject Nearest(Func<KeyValuePair<GameObject, Token>, bool> predicate) {
        IEnumerable<KeyValuePair<GameObject, Token>> predicateProof = _objectsInside.Where(predicate);
        float nearestDistance = Mathf.Infinity;
        GameObject nearest = null;
        foreach (KeyValuePair<GameObject, Token> obj in predicateProof) {
            float distance = Vector2.Distance(transform.Position2D(), obj.Key.transform.Position2D());
            if (distance < nearestDistance) {
                nearest = obj.Key;
                distance = nearestDistance;
            }
        }
        return nearest;
    }

    #region Callbacks

    private void _OnCollisionEnter(Collision2D collision) {
        if (_colliderGate != ColliderGate.HARD && _colliderGate != ColliderGate.ANY) { return; }
        EnterTheArea(collision.gameObject);
    }

    private void _OnTriggerEnter(Collider2D collider) {
        if (_colliderGate == ColliderGate.TRIGGER_HARD && collider.isTrigger) { return; }
        if (_colliderGate == ColliderGate.TRIGGER_TRIGGER && !collider.isTrigger) { return; }
        EnterTheArea(collider.gameObject);
    }

    private void _OnCollisionExit(Collision2D collision) {
        if (_colliderGate != ColliderGate.HARD && _colliderGate != ColliderGate.ANY) { return; }
        LeaveTheArea(collision.gameObject);
    }

    private void _OnTriggerExit(Collider2D collider) {
        if (_colliderGate == ColliderGate.TRIGGER_HARD && collider.isTrigger) { return; }
        if (_colliderGate == ColliderGate.TRIGGER_TRIGGER && !collider.isTrigger) { return; }
        LeaveTheArea(collider.gameObject);
    }

    #endregion
}
