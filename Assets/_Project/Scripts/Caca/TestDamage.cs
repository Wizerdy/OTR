using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDamage : MonoBehaviour
{
    private Rigidbody2D rb;
    public Health health;
    public int damage = 5;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        health.TakeDamage(damage);
    }
}
