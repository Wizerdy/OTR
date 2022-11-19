using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class overlapCircle : MonoBehaviour {
    public bool a;
    public bool b;
    public float radius;

    void Update() {
        if (a) {
            a = false;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in colliders) {
                Debug.Log(collider.gameObject);
            }
        }
        if (b) {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in colliders) {
                Debug.Log(collider.gameObject);
            }
        }
    }
}
