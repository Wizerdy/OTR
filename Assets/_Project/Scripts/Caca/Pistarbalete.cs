using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistarbalete : Weapon
{
    [SerializeField] GameObject bolt;
    [SerializeField] float _attackTime = 0.2f;
    [SerializeField] float _bulletSpeed = 5f;
    string _triggerName = "Bow_shoot";

    protected override IEnumerator IAttack(Vector2 direction) {
        //if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        //_targetAnimator.SetTrigger(_triggerName);
        Instantiate(bolt, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
        bolt.GetComponent<Rigidbody2D>().velocity = direction * _bulletSpeed;
        yield return new WaitForSeconds(_attackTime);
    }
}
