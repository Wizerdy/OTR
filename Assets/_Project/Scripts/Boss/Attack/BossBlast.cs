using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlast : BossAttack {
    [Header("Pas Touche:")]
    [SerializeField] protected Blast _blastPrefab;
    [Header("Blast :")]
    [SerializeField] protected float _chargeDuration;
    [SerializeField] protected float _blastDuration;
    [SerializeField] protected float _width;
    [SerializeField] protected float _secondWidth;
    [SerializeField] protected float _angle;
    [SerializeField] protected float _distFromBoss;
    [SerializeField] protected float _distBlast;
    [SerializeField] protected float _damagesMultiplier;

    protected override IEnumerator AttackBegins(EntityAbilities ea, Transform target) {
        _ea.Get<EntityBoss>().SetAnimationTrigger("Blasting");
        Blast(ea.transform.position, target.position);
        yield return new WaitForSeconds(_chargeDuration);
    }
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        yield return new WaitForSeconds(_blastDuration);
    }

    protected void Blast(Vector3 ourPosition, Vector3 target) {
        Vector2[] array = new Vector2[6];
        Vector2 direction = (target - ourPosition).normalized;
        array[0] = (direction * _distFromBoss) + (Vector2)(Quaternion.Euler(0, 0, 90f) * (direction * _width / 2));
        array[1] = (direction * _distFromBoss) + (Vector2)(Quaternion.Euler(0, 0, 90f) * (-direction * _width / 2));
        array[5] = array[0] + (Vector2)(Quaternion.Euler(0, 0, _angle - 90) * direction * _secondWidth);
        array[2] = array[1] + (Vector2)(Quaternion.Euler(0, 0, -(_angle - 90)) * direction * _secondWidth);
        array[4] = array[5] + (direction * _distBlast);
        array[3] = array[2] + (direction * _distBlast);
        Blast _newBlast = Instantiate(_blastPrefab, transform.position, Quaternion.identity).ChangeDamages(_damages).ChangeDamagesMultipler(_damagesMultiplier).ChangeBlastDuration(_blastDuration).ChangeChargeDuration(_blastDuration);
        _newBlast.ChangePoints(array);
    }
}
