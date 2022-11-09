using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : Weapon {
    [SerializeField] float _attackTime = 0.2f;
    [SerializeField] GameObject _bullet;
    [SerializeField] float _bulletSpeed = 5f;
    [SerializeField] int _damages = 5;
    string _triggerName = "Bow_shoot";

    protected override void _OnStart() {
        _attacks.Add(AttackIndex.FIRST, new WeaponAttack(_attackTime, _damages, 0, IAttack));
    }

    protected IEnumerator IAttack(EntityAbilities caster, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        _targetAnimator.SetTrigger(_triggerName);
        Instantiate(_bullet, transform.position, Quaternion.identity);
        _bullet.GetComponent<Rigidbody2D>().velocity = direction * _bulletSpeed;
        yield return new WaitForSeconds(_attackTime);
    }
}
