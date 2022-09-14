using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sloot {
    public class Pool<T> where T : MonoBehaviour {
        List<T> _pool;
        List<T> _alive;
        T _original;
        int _count;
        int _maxCount;
        GameObject _poolStorage;

        public int Count { get { return _count; } }
        public float MaxCount { get { return _maxCount; } }

        public Pool(T original) {
            _pool = new List<T>();
            _alive = new List<T>();
            _original = original;
            _count = 0;
            _maxCount = 2147483647;
        }

        public Pool(T original, GameObject poolStorage = null, int maxCount = 2147483647) {
            _pool = new List<T>();
            _alive = new List<T>();
            _original = original;
            _count = 0;
            if (maxCount < 0) {
                _maxCount = 0;
            } else {
                _maxCount = maxCount;
            }
            if (poolStorage != null) {
                GameObject storage = new GameObject();
                storage.transform.parent = poolStorage.transform;
                storage.name = "Storage";
                _poolStorage = storage;
            }
        }

        public T GetInstance() {
            T newObject;
            if (_pool.Count == 0) {
                if (_maxCount <= _count) {
                    return null;
                } else {
                    newObject = UnityEngine.Object.Instantiate(_original);
                    newObject.gameObject.name = typeof(T).FullName + " N°" + _count;
                    Reset(newObject);
                    _count++;
                    if (_poolStorage != null) {
                        newObject.gameObject.transform.parent = _poolStorage.transform;
                    }
                }
            } else {
                newObject = _pool[0];
                _pool.Remove(newObject);
            }
            _alive.Add(newObject);
            newObject.gameObject.SetActive(true);
            return newObject;
        }

        public void LetInstance(T instance) {
            if (!_alive.Contains(instance)) {
                throw new ArgumentException("instance is not recognize", instance.gameObject.name);
            } else {
                instance.gameObject.SetActive(false);
                Reset(instance);
                _alive.Remove(instance);
                _pool.Add(instance);
                if (_poolStorage != null) {
                    instance.gameObject.transform.parent = _poolStorage.transform;
                }
            }
        }

        public void ChangeMaxCount(int newMaxCount) {
            _maxCount = newMaxCount;
        }

        public bool Contains(T instance) {
            return _alive.Contains(instance) || _pool.Contains(instance);
        }

        void Reset(T instance) {
            (instance as IReset)?.Reset();
        }
    }
}
