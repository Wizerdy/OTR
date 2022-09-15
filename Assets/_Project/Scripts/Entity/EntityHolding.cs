using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class EntityHolding : MonoBehaviour, IEntityAbility {
    GameObject _holding;
    IHoldable _iholding;

    [SerializeField, HideInInspector] BetterEvent<GameObject> _onPickup = new BetterEvent<GameObject>();
    [SerializeField, HideInInspector] BetterEvent<GameObject> _onDrop = new BetterEvent<GameObject>();
    [SerializeField, HideInInspector] BetterEvent<GameObject, Vector2> _onThrow = new BetterEvent<GameObject, Vector2>();

    #region Properties

    public GameObject Holding { get => _holding; set => _holding = value; }
    public bool IsHolding => _holding != null;
    public IHoldable IHolding => _iholding;

    public event UnityAction<GameObject> OnPickup { add => _onPickup.AddListener(value); remove => _onPickup.RemoveListener(value); }
    public event UnityAction<GameObject> OnDrop { add => _onDrop.AddListener(value); remove => _onDrop.RemoveListener(value); }
    public event UnityAction<GameObject, Vector2> OnThrow { add => _onThrow.AddListener(value); remove => _onThrow.RemoveListener(value); }

    #endregion

    private void Start() {
        if (_holding != null) {
            if (_holding.GetComponent<IHoldable>() != null) {
                Pickup(_holding);
            } else {
                _holding = null;
            }
        }
    }

    public void Pickup(GameObject target) {
        if (target == _holding) { return; }
        Drop();
        _iholding = target.GetComponent<IHoldable>();
        if (_iholding == null) { Debug.LogWarning("Trying to pickup a not holdable item; " + target.name); return; }
        _iholding.Pickup(this);
        _holding = target;
    }

    public void Drop() {
        if (!IsHolding) { return; }
        _iholding.Drop(this, transform.position);
        _holding = null;
        _iholding = null;
    }

    public void Throw(Vector2 direction, Collider2D thrower = null) {
        if (!IsHolding) { return; }
        IHoldable item = _iholding;
        Drop();
        item.Throw(this, direction, thrower);
    }

    public void Throw(Vector2 direction, GameObject thrower) {
        if (!IsHolding) { return; }
        IHoldable item = _iholding;
        Drop();
        item.Throw(this, direction, thrower);
    }
}
