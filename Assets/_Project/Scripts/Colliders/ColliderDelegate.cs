using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class ColliderDelegate : MonoBehaviour {
    [SerializeField] bool _debug = false;
    [SerializeField] BetterEvent<Collision2D> _onCollisionEnter = new BetterEvent<Collision2D>();
    [SerializeField] BetterEvent<Collision2D> _onCollisionExit = new BetterEvent<Collision2D>();
    [SerializeField] BetterEvent<Collision2D> _onCollisionStay = new BetterEvent<Collision2D>();
    [SerializeField] BetterEvent<Collider2D> _onTriggerEnter = new BetterEvent<Collider2D>();
    [SerializeField] BetterEvent<Collider2D> _onTriggerExit = new BetterEvent<Collider2D>();
    [SerializeField] BetterEvent<Collider2D> _onTriggerStay = new BetterEvent<Collider2D>();

    bool _active = true;

    public bool Active { get => _active; set => _active = value; }

    public event UnityAction<Collision2D> OnCollisionEnter { add => _onCollisionEnter.AddListener(value); remove => _onCollisionEnter.RemoveListener(value); }
    public event UnityAction<Collision2D> OnCollisionExit { add => _onCollisionExit.AddListener(value); remove => _onCollisionExit.RemoveListener(value); }
    public event UnityAction<Collision2D> OnCollisionStay { add => _onCollisionStay.AddListener(value); remove => _onCollisionStay.RemoveListener(value); }
    public event UnityAction<Collider2D> OnTriggerEnter { add => _onTriggerEnter.AddListener(value); remove => _onTriggerEnter.RemoveListener(value); }
    public event UnityAction<Collider2D> OnTriggerExit { add => _onTriggerExit.AddListener(value); remove => _onTriggerExit.RemoveListener(value); }
    public event UnityAction<Collider2D> OnTriggerStay { add => _onTriggerStay.AddListener(value); remove => _onTriggerStay.RemoveListener(value); }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!_active) { return; }
        if (_debug) { DebugMe("Collision Enter " + collision.gameObject.name); }
        _onCollisionEnter.Invoke(collision);
    }

    private void OnCollisionStay2D(Collision2D collision) {
        if (!_active) { return; }
        if (_debug) { DebugMe("Collision Stay " + collision.gameObject.name); }
        _onCollisionStay.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (!_active) { return; }
        if (_debug) { DebugMe("Collision Exit " + collision.gameObject.name); }
        _onCollisionExit.Invoke(collision);
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        if (!_active) { return; }
        if (_debug) { DebugMe("Trigger Enter " + collider.gameObject.name); }
        _onTriggerEnter.Invoke(collider);
    }

    private void OnTriggerStay2D(Collider2D collider) {
        if (!_active) { return; }
        if (_debug) { DebugMe("Trigger Stay " + collider.gameObject.name); }
        _onTriggerStay.Invoke(collider);
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (!_active) { return; }
        if (_debug) { DebugMe("Trigger Exit " + collider.gameObject.name); }
        _onTriggerExit.Invoke(collider);
    }

    private void DebugMe(string message) {
        Debug.Log(name + " debug: " + message);
    }
}
