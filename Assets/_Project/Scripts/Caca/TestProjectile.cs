using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;

public class TestProjectile : MonoBehaviour, IReflectable
{
    [SerializeField] private float lifeSpan = 3.0f; 
    [SerializeField] private float speed = 3.0f; 
    private Rigidbody2D rb2D;

    public void Launch(float force, Vector2 direction) {
        rb2D.velocity = force * direction;
    }

    public void Reflect(ContactPoint2D collision) {
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = Vector2.left * speed;
    }

    // Update is called once per frame
    void Update()
    {
        lifeSpan -= Time.deltaTime;
        if (lifeSpan < 0) {
            Destroy(gameObject);
        }
    }
}
