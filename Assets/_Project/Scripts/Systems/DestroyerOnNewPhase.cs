using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyerOnNewPhase : MonoBehaviour {
    public static DestroyerOnNewPhase _instance;
    List<DestroyOnNewPhase> _toDestroyed;

    private void Awake() {
        if (_instance == null) {
            _instance = this;
        } else {
            Destroy(this.gameObject);
            return;
        }
        _toDestroyed = new List<DestroyOnNewPhase>();
    }

    public void DestroyAll() {
        for (int i = 0; i < _toDestroyed.Count; i++) {
            Destroy(_toDestroyed[i].gameObject);
        }
    }

    public void Add(DestroyOnNewPhase toAdd) {
        if (!_toDestroyed.Contains(toAdd)) {
            _toDestroyed.Add(toAdd);
        }
    }

    public void Remove(DestroyOnNewPhase toRemove) {
        if (_toDestroyed.Contains(toRemove)) {
            _toDestroyed.Remove(toRemove);
        }
    }
}
