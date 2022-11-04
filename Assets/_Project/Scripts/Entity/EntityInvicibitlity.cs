using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class EntityInvicibitlity : MonoBehaviour, IEntityAbility {
    [SerializeField] LayerMask _invicibleLayer;
    [SerializeField] List<Collider2D> _colliders;
    [SerializeField] Health _health;

    Dictionary<Collider2D, int> _collidersLayers = new Dictionary<Collider2D, int>();
    Coroutine _routine_layer = null;
    Coroutine _routine_invincible = null;

    private void Start() {
        for (int i = 0; i < _colliders.Count; i++) {
            _collidersLayers.Add(_colliders[i], _colliders[i].gameObject.layer);
        }
    }

    public void ChangeCollisionLayer(float time) {
        if (_routine_layer != null) { StopCoroutine(_routine_layer); }
        _routine_layer = StartCoroutine(IChangeCollisionLayer(time));
    }

    public void Invicible(float time) {
        _health.CanTakeDamage = false;
        if (_routine_invincible != null) { _health.CanTakeDamage = true; StopCoroutine(_routine_invincible); }
        _routine_invincible = StartCoroutine(Tools.Delay(() => _health.CanTakeDamage = true, time));
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
