using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Weapon {
    [SerializeField] DirectionalModifier _directionalModifier;
    [SerializeField] float _attackTime = 0.2f;
    //Vector2 _direction = Vector2.zero;

    string _boolName_aim = "Shield_aim";
    string _triggerName_attack = "Shield_bash";

    protected override void _OnAim(Vector2 direction) {
        if (direction == Vector2.zero) { _directionalModifier.enabled = false; _targetAnimator.SetBool(_boolName_aim, false); return; }
        _directionalModifier.enabled = true;
        _directionalModifier.Direction = direction;
        _targetAnimator.SetBool(_boolName_aim, true);
    }

    //protected override void _OnPickedUpdate() {
        
    //}

    protected override void _OnPickup(EntityWeaponry weaponry) {
        weaponry.Health.AddDamageModifier(_directionalModifier);
    }

    protected override void _OnDrop(EntityHolding holding) {
        _targetAnimator.SetBool(_boolName_aim, false);
    }

    protected override void _OnDrop(EntityWeaponry weaponry) {
        weaponry.Health.RemoveDamageModifier(_directionalModifier);
    }

    protected override IEnumerator IAttack(Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        _targetAnimator.SetTrigger(_triggerName_attack);
        yield return new WaitForSeconds(_attackTime);
    }
}
