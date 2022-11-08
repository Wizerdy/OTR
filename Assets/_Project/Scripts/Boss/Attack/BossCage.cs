using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using ToolsBoxEngine;

public class BossCage : BossAttack {
    [Header("Prog")]
    [SerializeField] Cage _cagePrefab;
    [SerializeField] Transform _topRight;
    [SerializeField] Transform _botLeft;
    [Header("GD")]
    [SerializeField] Vector2 _position;
    [SerializeField] Vector2 _size;
    [SerializeField] float _tick;
    [SerializeField] float _cageDuration;
    [SerializeField] int _bonusDamageEveryTick;
    float _weightMemory;

    protected override IEnumerator AttackBegins(EntityAbilities ea, Transform target) {
        _weightMemory = _weight;
        _weight = 0;
        StartCoroutine(Tools.Delay(() => { _weight = _weightMemory; }, _cageDuration));
        yield break;
    }
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform transform) {
        Cage newCage = Instantiate(_cagePrefab).ChangeBotLeft(_botLeft).ChangePosition(_position).ChangeSize(_size).ChangeTopRight(_topRight).ChangeDuration(_cageDuration).ChangeDamages(_damages).ChangeTick(_tick).ChangeDamagesBonus(_bonusDamageEveryTick);
        newCage.transform.position = _position;
        yield break;
    }
}
