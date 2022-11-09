using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Sword : Weapon {
    [SerializeField] int _damages = 10;
    [SerializeField] float _attackTime = 0.2f;
    [SerializeField] float _comboTimer = 0.2f;

    Coroutine _routine_ResetCombo = null;
    int _comboIndex = 0;

    string[] _triggerName = { "Sword_slash_1", "Sword_slash_2", "Sword_slash_3" };

    protected override void _OnStart() {
        _attacks.Add(AttackIndex.FIRST, new WeaponAttack(_attackTime, _damages, 0, IAttack));
    }

    protected IEnumerator IAttack(EntityAbilities caster, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        if (_routine_ResetCombo != null) { CoroutinesManager.Stop(_routine_ResetCombo); }

        _targetAnimator.SetTrigger(_triggerName[_comboIndex]);
        yield return new WaitForSeconds(_attackTime);
        ++_comboIndex;
        _comboIndex %= _triggerName.Length;
        _routine_ResetCombo = CoroutinesManager.Start(Tools.Delay(() => _comboIndex = 0, _comboTimer));
    }

    private void OnDestroy() {
        if (_routine_ResetCombo != null) { CoroutinesManager.Stop(_routine_ResetCombo); }
    }
}
