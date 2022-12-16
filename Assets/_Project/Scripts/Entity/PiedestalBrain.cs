using System;
using System.Collections;
using System.Collections.Generic;
using ToolsBoxEngine;
using UnityEngine;

public class PiedestalBrain : MonoBehaviour {
    [System.Serializable]
    struct TypedWeapon {
        public WeaponType type;
        public GameObject obj;
    }

    [SerializeField] EntityHolding _holding;
    [SerializeField] ColliderDelegate _collider;
    [SerializeField] List<TypedWeapon> _weapons;

    GameObject _lastDropped = null;
    Coroutine _routine_LastDropped = null;

    void Start() {
        Deactivate();
        _collider.OnTriggerEnter += _CollisionEnter;
        _holding.OnDrop += _OnDrop;
    }

    private void _OnDrop(GameObject _) {
        Deactivate();
    }

    private void Pickup(GameObject holdable) {
        if (_routine_LastDropped != null) { StopCoroutine(_routine_LastDropped); }

        _holding.Pickup(holdable);
        Weapon weapon = holdable.GetComponent<Weapon>();
        if (weapon == null) { return; }
        for (int i = 0; i < _weapons.Count; i++) {
            if (_weapons[i].type == weapon.Type) {
                _weapons[i].obj.SetActive(true);
            }
        }
    }

    private void GiveItem(EntityHolding target) {
        if (target == null) { return; }
        if (_holding.Holding == null) { return; }
        if (target.IsHolding) { return; }
        _lastDropped = _holding.Holding;
        _holding.Drop();
        target.Pickup(_holding.Holding);
        Deactivate();
        _routine_LastDropped = StartCoroutine(Tools.Delay(() => _lastDropped = null, 0.2f));
    }

    private void Deactivate() {
        for (int i = 0; i < _weapons.Count; i++) {
            _weapons[i].obj.SetActive(false);
        }
    }

    private void _CollisionEnter(Collider2D collider) {
        GameObject root = collider.gameObject.GetRoot();
        if (root == _lastDropped) { return; }
        if (!_holding.IsHolding) {
            Debug.Log(root.GetComponent<IHoldable>() + " .. " + root.GetComponent<IHoldable>()?.Landmaster + " .. " + (root.GetComponent<IHoldable>()?.Landmaster == null));
            if (root.GetComponent<IHoldable>()?.Landmaster == null) {
                Pickup(root);
                return;
            }
        } else {
            if (!collider.gameObject.CompareTag("Player")) { return; }
            EntityAbilities abilities = root.GetComponent<EntityAbilities>();
            if (abilities != null) {
                GiveItem(abilities.Get<EntityHolding>());
                return;
            }
        }
    }
}
