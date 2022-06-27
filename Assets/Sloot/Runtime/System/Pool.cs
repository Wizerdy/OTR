using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Sloot {
    public class Pool<T> where T : new() {
        List<T> pool = new List<T>();
        List<T> alive = new List<T>();

        public T GetInstance() {
            T newObject;
            if (pool.Count == 0) {
                newObject = new T();
            } else {
                newObject = pool[0];
                pool.RemoveAt(0);
            }
            alive.Add(newObject);
            return newObject;
        }

        public void GiveInstance(T instance) {
            if (!alive.Contains(instance)) {
                throw new ArgumentException("instance is not recognize", nameof(instance));
            } else {
                pool.Add(instance);
                alive.Remove(instance);
            }
        }
    }
}
