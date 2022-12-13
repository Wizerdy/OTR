using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine.BetterEvents;

public class PlayerCount : MonoBehaviour {
    [SerializeField] int _target = 3;
    [SerializeField] BetterEvent _onEveryone = new BetterEvent();

    int _count = 0;

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            ++_count;
            _count = Mathf.Min(_target, _count);
            Debug.Log(_count);
            if (_count >= _target) {
                _onEveryone.Invoke();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            --_count;
            _count = Mathf.Max(0, _count);
        }
    }
}
