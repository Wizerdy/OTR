using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class LaserCoda : MonoBehaviour {
    [SerializeField] float laserPower;
    [SerializeField] public BoxCollider2D Collider;
    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.transform.tag == "Player") {
            Debug.Log("aller");
            EntityMovement tamer = collision.gameObject.GetComponent<ColliderRoot>().Root.GetComponentInChildren<EntityMovement>();
            tamer.CanMove = false;
            collision.gameObject.GetComponent<ColliderRoot>().Root.GetComponent<Rigidbody2D>().velocity = (transform.position - FindObjectOfType<Coda>().transform.position).normalized * laserPower;
            Debug.Log(collision.gameObject.GetComponent<ColliderRoot>().Root.GetComponent<Rigidbody2D>().velocity);
            Debug.Log((transform.position - FindObjectOfType<Coda>().transform.position).normalized * laserPower);
            CoroutinesManager.Start(Tools.Delay(() => tamer.CanMove = true, 0.2f));
        }
    }
}
