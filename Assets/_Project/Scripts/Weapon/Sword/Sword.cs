using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon {
    [SerializeField] float _attackTime = 0.2f;
    string _triggerName = "Sword_slash";

    protected override IEnumerator IAttack(Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        _targetAnimator.SetTrigger(_triggerName);
        yield return new WaitForSeconds(_attackTime);
    }
}
