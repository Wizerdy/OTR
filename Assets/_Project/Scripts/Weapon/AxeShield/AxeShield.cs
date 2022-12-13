using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using UnityEngine.Events;

public class AxeShield : Weapon
{
    [Header("Slash")]
    [SerializeField] private int slashDamage = 10;
    [SerializeField] private int slashThreatGenerated = 1;
    [SerializeField] float _attackTime = 0.0f;
    [SerializeField] float attackCooldown = 0.2f;
    [Header("Parry")]
    [SerializeField] private float parryCooldown = 0.5f;
    [SerializeField] private int parryThreatGenerated = 0;
    [SerializeField] private int parryDamageReduction = 1;
    [SerializeField] private UnityEvent onParry = new UnityEvent();
    [Header("Armor")]
    [SerializeField] private float armorPointRegenerationRate = 1;
    [SerializeField] private int armorPointRegenerationValue = 1;
    [SerializeField] private int armorPointMax = 10;
    [SerializeField] private int armorPointOnPickUp = 5;

    Vector2 aimingDirection;
    public EntityArmor entityArmor;

    float _baseSpeed = 1f;
    bool _aiming = false;

    string _boolName_aim = "AxeShield_Slash";

    string _triggerName_attack_slash = "AxeShield_Slash";
    string _triggerName_attack_parry = "AxeShield_Parry";

    public int ArmorRegeneration { get => armorPointRegenerationValue; set { armorPointRegenerationValue = value; entityArmor.RegenValueArmor = armorPointRegenerationValue; } }

    protected override void _OnStart() {
        _baseSpeed = MoveSpeed;

        _attacks.Add(AttackIndex.SECOND, new WeaponAttack(parryCooldown, 0, parryThreatGenerated, IAttackParry));
        _attacks.Add(AttackIndex.FIRST, new WeaponAttack(_attackTime, slashDamage, slashThreatGenerated, IAttackSlash));
        _type = WeaponType.SHIELDAXE;
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
        //_targetAnimator.SetBool(_boolName_aim, false);
        _aiming = false;
        MoveSpeed = _baseSpeed;
    }

    protected override void _OnDrop(EntityWeaponry weaponry) {
        //armorPointCurrent = 0;
        entityArmor.ResetArmor();
    }

    private void StartAiming() {
        _aiming = true;
    }

    private void UpdateAim(Vector2 direction) {
        aimingDirection = direction;
    }

    private void StopAiming() {
        _aiming = false;
    }

    protected override void _OnAim(Vector2 direction) {
        if (direction == Vector2.zero) {
            if (_aiming) { StopAiming(); }
            return;
        }

        if (!_aiming) { StartAiming(); }
        UpdateAim(direction);
    }

    protected IEnumerator IAttackSlash(EntityAbilities caster, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }
        
        _targetAnimator.SetTrigger(_triggerName_attack_slash);
        _attacks[AttackIndex.FIRST].canAttack = false;
        CoroutinesManager.Start(Tools.Delay(() => _attacks[AttackIndex.FIRST].canAttack = true, attackCooldown));

        if (_attackTime > 0) {
            yield return new WaitForSeconds(_attackTime);
        }
    }

    protected IEnumerator IAttackParry(EntityAbilities caster, Vector2 direction) {
        if (_targetAnimator == null) { Debug.LogError(gameObject.name + " : Animator not set"); yield break; }

        onParry?.Invoke();
        _targetAnimator.SetTrigger(_triggerName_attack_parry);

        _attacks[AttackIndex.SECOND].canAttack = false;
        CoroutinesManager.Start(Tools.Delay(() => _attacks[AttackIndex.SECOND].canAttack = true, parryCooldown));

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
