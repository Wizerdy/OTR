using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeShield : Weapon
{
    [Header("Slash")]
    [SerializeField] private int slashDamage = 10;
    [SerializeField] private int slashThreatGenerated = 1;
    [SerializeField] float _attackTime = 0.2f;
    [Header("Parry")]
    [SerializeField] private float parryCooldown = 0.5f;
    [SerializeField] private int parryThreatGenerated = 0;
    [SerializeField] private int parryDamageReduction = 1;
    [Header("Armor")]
    [SerializeField] private float armorPointRegenerationRate = 1;
    [SerializeField] private int armorPointRegenerationValue = 1;
    [SerializeField] private int armorPointMax = 10;
    [SerializeField] private int armorPointOnPickUp = 5;


    private EntityArmor entityArmor;

    float _baseSpeed = 1f;
    //bool _aiming = false;

    string _boolName_aim = "AxeShield_Slash";

    string _triggerName_attack_slash = "AxeShield_Slash";
    string _triggerName_attack_parry = "AxeShield_Parry";
    private WeaponAttack attackSlash;
    private WeaponAttack attackParry;

    protected override void _OnStart() {
        _baseSpeed = MoveSpeed;

        attackSlash.attack = IAttackSlash;
        attackSlash.damage = slashDamage;
        attackSlash.attackTime = _attackTime;
        attackSlash.threatPoint = slashThreatGenerated;

        attackParry.attack = IAttackParry;
        attackParry.damage = 0;
        attackParry.attackTime = parryCooldown;
        attackParry.threatPoint = parryThreatGenerated;

        _attacks.Add(AttackIndex.FIRST, attackParry);
        _attacks.Add(AttackIndex.SECOND, attackSlash);
    }

    protected override void _OnPickup(EntityWeaponry weaponry) {
        //armorPointCurrent = armorPointOnPickUp;
        entityArmor = User.Get<EntityArmor>();

        entityArmor.OnPickUp();
        entityArmor.MaxArmor = armorPointMax;
        entityArmor.CurrentArmor = armorPointOnPickUp;
        entityArmor.RegenRateArmor = armorPointRegenerationRate;
        entityArmor.RegenValueArmor = armorPointRegenerationValue;
        entityArmor.ParryDamageReduction = parryDamageReduction;
    }

    protected override void _OnDrop(EntityHolding holding) {
        _targetAnimator.SetBool(_boolName_aim, false);
        //_aiming = false;
        MoveSpeed = _baseSpeed;
    }

    protected override void _OnDrop(EntityWeaponry weaponry) {
        //armorPointCurrent = 0;
        entityArmor.ResetArmor();
    }

    protected IEnumerator IAttackSlash(EntityAbilities caster, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        
        _targetAnimator.SetTrigger(_triggerName_attack_slash);
        yield return new WaitForSeconds(_attackTime);
    }

    protected IEnumerator IAttackParry(EntityAbilities caster, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        _targetAnimator.SetTrigger(_triggerName_attack_parry);
        yield return new WaitForSeconds(parryCooldown);
    }

    //public override float AttackTime(AttackIndex index) {
    //    switch (index) {
    //        case AttackIndex.FIRST:
    //            return _attackTime;
    //        case AttackIndex.SECOND:
    //            return _attackTime;
    //        default:
    //            return 0f;
    //    }
    //}

}
