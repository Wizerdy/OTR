using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class ColliderDelegate : MonoBehaviour {
    [SerializeField] BetterEvent<Collision2D> _onCollisionEnter = new BetterEvent<Collision2D>();
    [SerializeField] BetterEvent<Collision2D> _onCollisionExit = new BetterEvent<Collision2D>();
    [SerializeField] BetterEvent<Collider2D> _onTriggerEnter = new BetterEvent<Collider2D>();
    [SerializeField] BetterEvent<Collider2D> _onTriggerExit = new BetterEvent<Collider2D>();

    public event UnityAction<Collision2D> OnCollisionEnter { add => _onCollisionEnter.AddListener(value); remove => _onCollisionEnter.RemoveListener(value); }
    public event UnityAction<Collision2D> OnCollisionExit { add => _onCollisionExit.AddListener(value); remove => _onCollisionExit.RemoveListener(value); }
    public event UnityAction<Collider2D> OnTriggerEnter { add => _onTriggerEnter.AddListener(value); remove => _onTriggerEnter.RemoveListener(value); }
    public event UnityAction<Collider2D> OnTriggerExit { add => _onTriggerExit.AddListener(value); remove => _onTriggerExit.RemoveListener(value); }

    private void OnCollisionEnter2D(Collision2D collision) {
        _onCollisionEnter.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        _onCollisionExit.Invoke(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        _onTriggerEnter.Invoke(collision);
    }

    private void OnTriggerExit2D(Collider2D collision) {
        _onTriggerExit.Invoke(collision);
    }
}
