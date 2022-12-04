using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Meteor : MonoBehaviour {
    [SerializeField] int _damages;
    [SerializeField] float _timeBeforeFall;
    List<IHealth> _hit = new List<IHealth>();
    Animator _animator;
    public Meteor ChangeDamages(int damages) {
        _damages = damages;
        return this;
    }

    public Meteor ChangeTimeBeforeFall(float timeBeforeFall) {
        _timeBeforeFall = timeBeforeFall;
        return this;
    }

    void Start() {
        _animator = GetComponent<Animator>();
        StartCoroutine(Sparkkk());

    }


    //private void OnTriggerStay2D(Collider2D collision) {
    //    IHealth health = collision.gameObject.GetRoot().GetComponent<IHealth>();
    //    if (health != null && !_hit.Contains(health) && collision.CompareTag("Player")) {
    //        health.TakeDamage(_damages, gameObject);
    //        _hit.Add(health);
    //    }
    //}

    void OnTriggerStay2D(Collider2D collision) {
        IHealth health = collision.gameObject.GetRoot().GetComponent<IHealth>();
        if (health != null && collision.CompareTag("Player") && !_hit.Contains(health)) {
            health.TakeDamage(_damages, gameObject);
            _hit.Add(health);
        }
    }

    IEnumerator Sparkkk() {
        yield return new WaitForSeconds(_timeBeforeFall);
        GetComponent<Collider2D>().enabled = true;
        _animator.SetTrigger("Spark");
    }
}
