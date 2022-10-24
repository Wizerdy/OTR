using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class FrameCollider : MonoBehaviour {
    [System.Serializable]
    public class Indexed<T> {
        public T value;
        public int index;

        public Indexed(int index, T value) {
            this.index = index;
            this.value = value;
        }
    }

    public interface IOverlapCollider2D {
        Collider2D[] Overlap(Transform parent);
    }

    [System.Serializable]
    public class Circle : IOverlapCollider2D {
        public Vector2 position;
        public float radius;

        public Circle(Vector2 position, float radius) {
            this.position = position;
            this.radius = radius;
        }

        public Collider2D[] Overlap(Transform parent) {
            return Physics2D.OverlapCircleAll(position + parent.Position2D(), radius);
        }
    }

    [System.Serializable]
    public class Rectangle : IOverlapCollider2D {
        public Vector2 position;
        public Vector2 size;
        public float rotation;

        public Rectangle(Vector2 position, Vector2 size, float rotation) {
            this.position = position;
            this.size = size;
            this.rotation = rotation;
        }

        public Collider2D[] Overlap(Transform parent) {
            return Physics2D.OverlapBoxAll(position + parent.Position2D(), size, rotation);
        }
    }

    [SerializeField] List<Indexed<Circle>> _overlapCircle;
    [SerializeField] List<Indexed<Rectangle>> _overlapRectangle;

    [SerializeField] int _currentIndex = 0;

    private void Reset() {

    }

    private void FixedUpdate() {
        if (_currentIndex < 0) { return; }

        Collider2D[] colliders = Overlap();
        for (int i = 0; i < colliders.Length; i++) {
            SendMessage("OnTriggerEnter2D", colliders[i]);
        }
    }

    public Collider2D[] Overlap() {
        if (_currentIndex < 0) { return null; }
        List<Collider2D> output = new List<Collider2D>();
        Collider2D[] colliders;

        for (int i = 0; i < _overlapCircle.Count; i++) {
            if (_overlapCircle[i].index != _currentIndex) { continue; }

            colliders = _overlapCircle[i].value.Overlap(transform);
            for (int j = 0; j < colliders.Length; j++) {
                if (!output.Contains(colliders[j])) {
                    output.Add(colliders[j]);
                }
            }
        }
        for (int i = 0; i < _overlapRectangle.Count; i++) {
            if (_overlapRectangle[i].index != _currentIndex) { continue; }

            colliders = _overlapCircle[i].value.Overlap(transform);
            for (int j = 0; j < colliders.Length; j++) {
                if (!output.Contains(colliders[j])) {
                    output.Add(colliders[j]);
                }
            }
        }

        return output.ToArray();
    }
}
