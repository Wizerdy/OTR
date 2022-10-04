using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeShield : Weapon
{
    [SerializeField] private float aggroPointGenerated = 0.5f;
    [SerializeField] private float parryCooldown = 0.5f;
    [SerializeField] private float armorPointCurrent = 10f;
    [SerializeField] private float armorPointRegenerationRate = 1.5f;
    [SerializeField] private float armorPointRegenerationValue = 1f;
    [SerializeField] private float armorPointMax = 10f;
    [SerializeField] private float armorPointOnPickUp = 5f;
    [SerializeField] float _attackTime = 0.2f;

    float _baseSpeed = 1f;
    bool _aiming = false;

    string _boolName_aim = "AxeShield_Slash";

    string _triggerName_attack_slash = "AxeShield_Slash";
    string _triggerName_attack_parry = "AxeShield_Parry";

    protected override void _OnStart() {
        _baseSpeed = MoveSpeed;
        _attacks.Add(AttackIndex.FIRST, IAttackSlash);
        _attacks.Add(AttackIndex.SECOND, IAttackParry);
    }

    protected override void _OnPickup(EntityWeaponry weaponry) {
        armorPointCurrent = armorPointOnPickUp;
    }

    protected override void _OnDrop(EntityHolding holding) {
        _targetAnimator.SetBool(_boolName_aim, false);
        _aiming = false;
        MoveSpeed = _baseSpeed;
    }

    protected override void _OnDrop(EntityWeaponry weaponry) {
        armorPointCurrent = 0;
    }

    protected IEnumerator IAttackSlash(EntityAbilities caster, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        
        //User.Get<EntityArmor>().armor = armorPointCurrent;
        _targetAnimator.SetTrigger(_triggerName_attack_slash);
        yield return new WaitForSeconds(_attackTime);
    }

    protected IEnumerator IAttackParry(EntityAbilities caster, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        _targetAnimator.SetTrigger(_triggerName_attack_parry);
        yield return new WaitForSeconds(_attackTime);
    }
}
