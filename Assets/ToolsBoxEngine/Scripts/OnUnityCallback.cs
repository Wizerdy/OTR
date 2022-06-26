using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToolsBoxEngine {
    public class OnUnityCallback : MonoBehaviour {
        [System.Serializable]
        enum UnityCallback { AWAKE, ENABLE, START, UPDATE, DISABLE, DESTROY }

        [SerializeField] UnityCallback _callback;
        [Space]
        [SerializeField] UnityEvent<GameObject> _action;

        void Awake() {
            if (_callback == UnityCallback.AWAKE) { _action?.Invoke(gameObject); }
        }

        void OnEnable() {
            if (_callback == UnityCallback.ENABLE) { _action?.Invoke(gameObject); }
        }

        void Start() {
            if (_callback == UnityCallback.START) { _action?.Invoke(gameObject); }
        }

        void Update() {
            if (_callback == UnityCallback.UPDATE) { _action?.Invoke(gameObject); }
        }

        void OnDisable() {
            if (_callback == UnityCallback.DISABLE) { _action?.Invoke(gameObject); }
        }

        void OnDestroy() {
            if (_callback == UnityCallback.DESTROY) { _action?.Invoke(gameObject); }
        }
    }
}