using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlast : BossAttack {
    [Header("Pas Touche:")]
    [SerializeField] Blast _blastPrefab;
    [Header("Blast :")]
    [SerializeField] float _chargeDuration;
    [SerializeField] float _blastDuration;
    [SerializeField] float _width;
    [SerializeField] float _secondWidth;
    [SerializeField] float _angle;
    [SerializeField] float _distFromBoss;
    [SerializeField] float _distBlast;
    [SerializeField] float _damagesMultiplier;

    protected override IEnumerator AttackBegins(EntityAbilities ea, Transform target) {
        yield return new WaitForSeconds(_chargeDuration);
    }
    protected override IEnumerator Attack(EntityAbilities ea, Transform target) {
        Blast(ea.transform.position, target);
        yield return new WaitForSeconds(_blastDuration);
    }

    void Blast(Vector3 ourPosition, Transform target) {
        Vector2[] array = new Vector2[6];
        Vector2 direction = (target.position - ourPosition).normalized;
        array[0] = (direction * _distFromBoss) + (Vector2)(Quaternion.Euler(0, 0, 90f) * (direction * _width / 2));
        array[1] = (direction * _distFromBoss) + (Vector2)(Quaternion.Euler(0, 0, 90f) * (-direction * _width / 2));
        array[5] = array[0] + (Vector2)(Quaternion.Euler(0, 0, _angle - 90) * direction * _secondWidth);
        array[2] = array[1] + (Vector2)(Quaternion.Euler(0, 0, -(_angle - 90)) * direction * _secondWidth);
        array[4] = array[5] + (direction * _distBlast);
        array[3] = array[2] + (direction * _distBlast);
        Blast _newBlast = Instantiate(_blastPrefab, transform.position, Quaternion.identity).ChangeDamages(_damages).ChangeDamagesMultipler(_damagesMultiplier).ChangeDuration(_blastDuration);
        _newBlast.ChangePoints(array);
    }
}
