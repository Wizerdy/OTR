using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBlast : BossAttack {
    [SerializeField] Blast _blastPrefab;
    [SerializeField] float _angle;
    [SerializeField] float _range;
    [SerializeField] float _duration;

    public override void Activate(EntityAbilities ea, Transform target) {
        Vector2[] array = new Vector2[4];
        array[0] = transform.position;
        array[3] = transform.position;
        Vector2 direction = (target.position - ea.transform.position).normalized;
        array[1] = Quaternion.Euler(new Vector3(0, 0, _angle / 2)) * direction * _range;
        array[2] = Quaternion.Euler(new Vector3(0, 0, -_angle / 2)) * direction * _range;
        Blast _newBlast = Instantiate(_blastPrefab, transform.position, Quaternion.identity);
        _newBlast.ChangePoints(array);
    }

    public override void Activate(EntityAbilities ea, Transform[] targets) {

    }
}
