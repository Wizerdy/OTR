using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class ReflectiveCollider : MonoBehaviour {
    //[SerializeField] bool _launchIt = false;
    [SerializeField] float _force = 4f;
    Vector2 _aimingDirection = Vector2.up;

    public Vector2 Aim { get => _aimingDirection; set => _aimingDirection = value; }

    //private void OnTriggerEnter2D(Collider2D collision) {
    //    IReflectable reflectable = collision.gameObject.GetComponentInRoot<IReflectable>();
    //    if (reflectable == null) { return; }
    //    //if (_launchIt) {
    //    if (_aimingDirection == Vector2.zero)
    //        reflectable.Launch(_force, Vector2.up);
    //    else
    //        reflectable.Launch(_force, _aimingDirection);
    //    //} else {
    //    //    Debug.Log("Reflect : " + collision.gameObject.name);
    //    //    reflectable.Reflect(collision.GetContact(0));
    //    //}
    //}
    private void OnCollisionEnter2D(Collision2D collision) {
        IReflectable reflectable = collision.gameObject.GetComponentInRoot<IReflectable>();
        if (reflectable == null) { return; }
        //if (_launchIt) {
        if (_aimingDirection == Vector2.zero)
            reflectable.Launch(_force, Vector2.up);
        else 
            reflectable.Launch(_force, _aimingDirection);
        //} else {
        //    Debug.Log("Reflect : " + collision.gameObject.name);
        //    reflectable.Reflect(collision.GetContact(0));
        //}
    }
}