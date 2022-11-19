using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class BossMeteor : BossAttack {
    [Header("Prog")]
    [SerializeField] Meteor _meteorPrefab;
    [SerializeField] Transform _topLeft;
    [SerializeField] Transform _botRight;
    [Header("GD")]
    [SerializeField] float _minimumDistBetween;
    [SerializeField] float _minimumDistFromSide;
    [SerializeField] float _number;
    [SerializeField] float _timeBeforeFall;
    int nameMeteor = 0;


    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        Physics2D.queriesHitTriggers = true;
        for (int i = 0; i < _number; i++) {
            MeteorFall();
        }
        Physics2D.queriesHitTriggers = false;
        yield break;
    }

    void MeteorFall() {
        Meteor meteor = Instantiate(_meteorPrefab).ChangeTimeBeforeFall(_timeBeforeFall).ChangeDamages(_damages);
        bool placed = false;
        Vector3 position = Vector3.zero;
        int protection = 0;
        while (!placed && protection <= 10) {
            placed = true;
            float x = Random.Range(_topLeft.position.x + _minimumDistFromSide, _botRight.position.x - _minimumDistFromSide);
            float y = Random.Range(_topLeft.position.y - _minimumDistFromSide, _botRight.position.y + _minimumDistFromSide);
            position = new Vector3(x, y, 0);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, _minimumDistBetween);
            foreach (Collider2D collider in colliders) {
                if (collider.transform.GetComponent<Meteor>() != null) {
                    placed = false;
                    break;
                }
            }
            protection++;
        }
        meteor.transform.position = position;
        meteor.name = "Meteor " + nameMeteor;
        nameMeteor++;
    }
}
