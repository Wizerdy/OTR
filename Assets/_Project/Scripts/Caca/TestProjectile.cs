using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestProjectile : MonoBehaviour, IReflectable
{
    private Rigidbody2D rb2D;

    public void Launch(float force, Vector2 direction) {
        rb2D.velocity = force * direction;
    }

    public void Reflect(ContactPoint2D collision) {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        rb2D.velocity = Vector2.left;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
