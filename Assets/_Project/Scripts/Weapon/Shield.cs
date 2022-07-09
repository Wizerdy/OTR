using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : Weapon {
    [SerializeField] float _attackTime = 0.2f;



    protected override IEnumerator IAttack(Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        _targetAnimator.SetTrigger("Shield");
        yield return new WaitForSeconds(_attackTime);
    }
}
