using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Pistarbalete : Weapon {
    [Header("Primary Attack")]
    [SerializeField] float _attackTime = 0.2f;
    [Header("Bolt")]
    [SerializeField] GameObject _bolt;
    [SerializeField] int _boltDamage = 5;
    [SerializeField] int _boltDamageBonus = 2;
    [SerializeField] int _boltCooldown = 1;
    [SerializeField] float _boltSpeed = 5f;

    [Header("Secondary Attack")]
    [SerializeField] GameObject specialBolt;
    [SerializeField] float _sbAttackTime = 2.0f;
    [SerializeField] float _sbSpeed = 5f;

    protected override void _OnStart() {
        _attacks.Add(AttackIndex.FIRST, IAttack);
        _attacks.Add(AttackIndex.SECOND, IAttack2);
    }

    protected IEnumerator IAttack(EntityAbilities entityAbilities, Vector2 direction) {
        //if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        //_targetAnimator.SetTrigger(_triggerName);
        GameObject go = Instantiate(_bolt, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
        //go.GetComponent<Rigidbody2D>().velocity = direction * _boltSpeed;
        go.GetComponent<Bolt>()?.SetDamage(_boltDamage)?.SetSpeed(_boltSpeed)?.SetDirection(direction);
        go.GetComponent<AditionalDamageByDistance>().PerfectDamage = _boltDamageBonus;
        _canAttack = false;
        CoroutinesManager.Start(Tools.Delay(() => _canAttack = true, _boltCooldown));
        yield return new WaitForSeconds(_attackTime);
    }

    protected IEnumerator IAttack2(EntityAbilities entityAbilities, Vector2 direction) {
        //if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        //_targetAnimator.SetTrigger(_triggerName);
        GameObject go = Instantiate(specialBolt, transform.position, Quaternion.LookRotation(Vector3.forward, direction));
        go.GetComponent<Rigidbody2D>().velocity = direction * _sbSpeed;
        yield return new WaitForSeconds(_sbAttackTime);
    }
}
