using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine.BetterEvents;

public class HoldingItems : MonoBehaviour {
    [SerializeField] List<EntityHolding> _holdings;
    [SerializeField] BetterEvent<bool> _onChange = new BetterEvent<bool>();

    int _count = 0;

    void Start() {
        for (int i = 0; i < _holdings.Count; i++) {
            _holdings[i].OnPickup += _Increment;
            _holdings[i].OnDrop += _Decrement;
            if (_holdings[i].IsHolding) {
                _Increment(null);
            }
        }
    }

    private void _Decrement(GameObject _) {
        --_count;
        _onChange.Invoke(_count >= _holdings.Count);
    }

    private void _Increment(GameObject _) {
        ++_count;
        _onChange.Invoke(_count >= _holdings.Count);
    }

    private void OnDestroy() {
        for (int i = 0; i < _holdings.Count; i++) {
            _holdings[i].OnPickup -= _Increment;
        }
    }
}
