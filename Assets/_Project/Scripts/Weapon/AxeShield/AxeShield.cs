using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeShield : Weapon
{
    public int axeShieldSlashDamage = 2;
    [SerializeField] private float aggroPointGenerated = 0.5f;
    [SerializeField] private float parryCooldown = 0.5f;
    //[SerializeField] private int armorPointCurrent = 10;
    [SerializeField] private float armorPointRegenerationRate = 1;
    [SerializeField] private int armorPointRegenerationValue = 1;
    [SerializeField] private int armorPointMax = 10;
    [SerializeField] private int armorPointOnPickUp = 5;
    [SerializeField] float _attackTime = 0.2f;

    private EntityArmor entityArmor;

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
        //armorPointCurrent = armorPointOnPickUp;

        entityArmor = User.Get<EntityArmor>();

        entityArmor.OnPickUp();
        entityArmor.SetMaxArmor(armorPointMax);
        entityArmor.SetCurrentArmor(armorPointOnPickUp);
        entityArmor.SetRegenRate(armorPointRegenerationRate);
        entityArmor.SetRegenValue(armorPointRegenerationValue);
    }

    protected override void _OnDrop(EntityHolding holding) {
        _targetAnimator.SetBool(_boolName_aim, false);
        _aiming = false;
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
        yield return new WaitForSeconds(_attackTime);
    }
}
