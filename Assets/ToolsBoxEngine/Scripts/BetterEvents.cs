using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace ToolsBoxEngine {
    [Serializable]
    public class BetterEvent<T1, T2> {
        [Space]
        [SerializeField] UnityEvent<T1, T2> _event;

        public event UnityAction<T1, T2> Event { add => _event.AddListener(value); remove => _event.RemoveListener(value); }

        public void Invoke(T1 argument1, T2 argument2) {
            try {
                _event?.Invoke(argument1, argument2);
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        public void AddListener(UnityAction<T1, T2> action) {
            _event.AddListener(action);
        }

        public void RemoveListener(UnityAction<T1, T2> action) {
            _event.RemoveListener(action);
        }

        public static BetterEvent<T1, T2> operator +(BetterEvent<T1, T2> e, UnityAction<T1, T2> action) {
            e.AddListener(action);
            return e;
        }

        public static BetterEvent<T1, T2> operator -(BetterEvent<T1, T2> e, UnityAction<T1, T2> action) {
            e.RemoveListener(action);
            return e;
        }
    }

    [Serializable]
    public class BetterEvent<T> {
        [Space]
        [SerializeField] UnityEvent<T> _event;

        public event UnityAction<T> Event { add => _event.AddListener(value); remove => _event.RemoveListener(value); }

        public void Invoke(T argument) {
            try {
                _event?.Invoke(argument);
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        public void AddListener(UnityAction<T> action) {
            _event.AddListener(action);
        }

        public void RemoveListener(UnityAction<T> action) {
            _event.RemoveListener(action);
        }
    }

    [Serializable]
    public class BetterEvent {
        [Space]
        [SerializeField] UnityEvent _event;

        public event UnityAction Event { add => _event.AddListener(value); remove => _event.RemoveListener(value); }

        public void Invoke() {
            try {
                _event?.Invoke();
            } catch (Exception e) {
                Debug.LogException(e);
            }
        }

        public void AddListener(UnityAction action) {
            _event.AddListener(action);
        }

        public void RemoveListener(UnityAction action) {
            _event.RemoveListener(action);
        }
    }

}