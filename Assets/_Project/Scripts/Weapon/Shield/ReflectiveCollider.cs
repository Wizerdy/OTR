using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;
using UnityEngine.Events;

public class ReflectiveCollider : MonoBehaviour {
    [SerializeField] bool _launchIt = false;
    [SerializeField] float _force = 4f;
    [SerializeField] float reflectedProjectileDamage = 10f;
    [SerializeField] ParticleSystem parryVFX;

    Vector2 _aimingDirection = Vector2.up;

    [SerializeField] protected BetterEvent<Collision2D> _onParry = new BetterEvent<Collision2D>();
    [SerializeField] protected BetterEvent<Collider2D> _onParryTrigger = new BetterEvent<Collider2D>();

    public Vector2 Aim { get => _aimingDirection; set => _aimingDirection = value; }

    private void OnTriggerEnter2D(Collider2D collision) {
        IReflectable reflectable = collision.gameObject.GetComponentInRoot<IReflectable>();
        if (reflectable == null) { return; }
        if (_launchIt) {
            reflectable.Launch(_force, _aimingDirection);
            _onParryTrigger.Invoke(collision);
            if (parryVFX.isPlaying)
                parryVFX.Stop();

            parryVFX.Play();
            //StartCoroutine(StopParticleSystem(parryVFXDuration));
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        IReflectable reflectable = collision.gameObject.GetComponentInRoot<IReflectable>();
        if (reflectable == null) { return; }
        if (_launchIt) {
            reflectable.Launch(_force, _aimingDirection);
            _onParry.Invoke(collision);
            if (parryVFX.isPlaying)
                parryVFX.Stop();

            parryVFX.Play();
        } else {
            //Debug.Log("Reflect : " + collision.gameObject.name);
            //reflectable.Reflect(collision.GetContact(0));

        }
    }

    //IEnumerator StopParticleSystem(float time) {
    //    yield return new WaitForSeconds(time);
    //    parryVFX.Stop();
    //}
}