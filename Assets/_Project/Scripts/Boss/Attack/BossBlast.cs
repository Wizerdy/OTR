using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlast : BossAttack {
    [Header("Blast :")]
    [SerializeField] Blast _blastPrefab;
    [SerializeField] float _width;
    [SerializeField] float _secondWidth;
    [SerializeField] float _angle;
    [SerializeField] float _distFromBoss;
    [SerializeField] float _distBlast;
    [SerializeField] int _damages;
    [SerializeField] float _damagesMultiplier;

    protected override IEnumerator Attack(EntityAbilities ea, Transform target) {
        Blast(ea.transform.position, target);
        yield return EndAttack();
    }

    void Blast(Vector3 ourPosition, Transform target) {
        Vector2[] array = new Vector2[6];
        Vector2 direction = (target.position - ourPosition).normalized;
        array[0] = (direction * _distFromBoss) + (Vector2)(Quaternion.Euler(0, 0, 90f) * (direction * _width / 2));
        // array[6] = (direction * _distFromBoss) + (Vector2)(Quaternion.Euler(0, 0, 90f) * (direction * _width / 2));
        array[1] = (direction * _distFromBoss) + (Vector2)(Quaternion.Euler(0, 0, 90f) * (-direction * _width / 2));
        array[5] = array[0] + (Vector2)(Quaternion.Euler(0, 0, _angle - 90) * direction * _secondWidth);
        array[2] = array[1] + (Vector2)(Quaternion.Euler(0, 0, -(_angle - 90)) * direction * _secondWidth);
        array[4] = array[5] + (direction * _distBlast);
        array[3] = array[2] + (direction * _distBlast);
        Blast _newBlast = Instantiate(_blastPrefab, transform.position, Quaternion.identity).ChangeDamages(_damages).ChangeDamagesMultipler(_damagesMultiplier);
        _newBlast.ChangePoints(array);
    }
}
