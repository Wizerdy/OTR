using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class EntityInvincibility : MonoBehaviour, IEntityAbility {
    [SerializeField] LayerMask _invicibleLayer;
    [SerializeField] List<Collider2D> _colliders;
    [SerializeField] Health _health;

    bool _invincible = false;

    Dictionary<Collider2D, int> _collidersLayers = new Dictionary<Collider2D, int>();
    Coroutine _routine_layer = null;
    Coroutine _routine_invincible = null;

    private void Start() {
        for (int i = 0; i < _colliders.Count; i++) {
            _collidersLayers.Add(_colliders[i], _colliders[i].gameObject.layer);
        }
    }

    public void ChangeCollisionLayer(float time) {
        if (time <= 0f) { return; }
        if (_routine_layer != null) { StopCoroutine(_routine_layer); }
        _routine_layer = StartCoroutine(IChangeCollisionLayer(time));
    }

    public void Invincible(bool state) {
        if (_invincible == state) { return; }
        _invincible = state;
        _health.CanTakeDamage = !_invincible;
    }

    public void Invincible(float time) {
        if (time <= 0f) { return; }
        Invincible(true);
        if (_routine_invincible != null) { StopCoroutine(_routine_invincible); }
        _routine_invincible = StartCoroutine(Tools.Delay(() => Invincible(false), time));
    }

    IEnumerator IChangeCollisionLayer(float time) {
        for (int i = 0; i < _colliders.Count; i++) {
            _colliders[i].gameObject.layer = _invicibleLayer;
        }
        yield return new WaitForSeconds(time);
        for (int i = 0; i < _colliders.Count; i++) {
            _colliders[i].gameObject.layer = _collidersLayers[_colliders[i]];
        }
    }
}
