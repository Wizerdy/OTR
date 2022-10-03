using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class BloodyFist : Weapon {
    [Header("Fist")]
    [SerializeField] float _lifeSteal;
    [SerializeField] float _hitdamage;
    [SerializeField] float _hitcooldown;
    [SerializeField] float _hitThreatPoint;
    [SerializeField] float _lifePointHealByHit;
    [SerializeField] float _storePointCostByHit;
    [SerializeField] float _storePointMax;

    [Header("Dash")]
    [SerializeField] float _storePointCost;
    [SerializeField] float _lifePointCost;
    [SerializeField] float _cooldown;
    [SerializeField] float _speed;
    [SerializeField] float _duration;
    [SerializeField] AnimationCurve _accelerationCurve;

    protected override void _OnStart() {
        _attacks.Add(AttackIndex.FIRST, FirstAttack);
        _attacks.Add(AttackIndex.SECOND, SecondAttack);
    }

    protected IEnumerator FirstAttack(EntityAbilities ea, Vector2 direction) {
        yield return null;
    }

    protected IEnumerator SecondAttack(EntityAbilities ea, Vector2 direction) {
        EntityMovement entityMovement = ea.Get<EntityMovement>();
        entityMovement.CreateMovement(_duration, _speed, _accelerationCurve);
        yield return null;
    }


}
