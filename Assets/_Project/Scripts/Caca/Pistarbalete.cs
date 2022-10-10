using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistarbalete : Weapon
{
    [Header("Primary Attack")]
    [SerializeField] GameObject bolt;
    [SerializeField] float _attackTime = 0.2f;
    [SerializeField] float _boltSpeed = 5f;
    string _triggerName = "Arbalete_shoot";

    [Header("Secondary Attack")]
    [SerializeField] GameObject specialBolt;
    [SerializeField] float _sbAttackTime = 2.0f;
    [SerializeField] float _sbSpeed = 5f;

    protected override void _OnStart() {
        _attacks.Add(AttackIndex.FIRST, IAttack);
        _attacks.Add(AttackIndex.SECOND, IAttack2);
    }

    protected IEnumerator IAttack(EntityAbilities entityAbilities, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        _targetAnimator.SetTrigger(_triggerName);
        GameObject go = Instantiate(bolt, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
        go.GetComponent<Rigidbody2D>().velocity = direction * _boltSpeed;
        yield return new WaitForSeconds(_attackTime);
    }

    protected IEnumerator IAttack2(EntityAbilities entityAbilities, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        _targetAnimator.SetTrigger(_triggerName);
        GameObject go = Instantiate(specialBolt, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
        go.GetComponent<Rigidbody2D>().velocity = direction * _sbSpeed;
        yield return new WaitForSeconds(_sbAttackTime);
    }
}
