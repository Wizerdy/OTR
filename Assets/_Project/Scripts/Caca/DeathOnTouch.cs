using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathOnTouch : MonoBehaviour {
    private void OnTriggerEnter2D(Collider2D collision) {
        Death(collision.gameObject);
    }

    private void Death(GameObject obj) {
        IHealth health;
        if (obj.GetComponentInRoot(out health) != null) {
            health.TakeDamage(10000, gameObject);
            Debug.Log(obj.GetRoot().name);
        }
    }
}
