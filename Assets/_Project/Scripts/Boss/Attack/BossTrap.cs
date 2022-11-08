using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class BossTrap : BossAttack {
    [Header("Prog")]
    [SerializeField] Trap _trapPrefab;
    [SerializeField] Transform _topRight;
    [SerializeField] Transform _botLeft;
    [Header("GD")]
    [SerializeField] float _minimumDistBetween;
    [SerializeField] float _minimumDistFromSide;
    [SerializeField] float _trapNumber;
    [SerializeField] float _visibleTime;
    [SerializeField] float _tick;


    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        for (int i = 0; i < _trapNumber; i++) {
            PutTrap();
        }
        yield break;
    }

    void PutTrap() {
        Trap trap = Instantiate(_trapPrefab).ChangeVisibility(_visibleTime).ChangeDamages(_damages).ChangeTick(_tick);
        bool placed = false;
        Vector2 position = Vector2.zero;
        int protection = 0;
        while (!placed || protection >= 1000) {
            placed = true;
            float x = Random.Range(_topRight.position.x + _minimumDistFromSide, _botLeft.position.x - _minimumDistFromSide);
            float y = Random.Range(_topRight.position.y - _minimumDistFromSide, _botLeft.position.y + _minimumDistFromSide);
            position = new Vector2(x, y);
            Collider2D[] colliders = Physics2D.OverlapCircleAll(position, _minimumDistBetween);
            foreach (Collider2D collider in colliders) {
                if(collider.gameObject.GetComponent<Trap>() != null) {
                    placed = false;
                }
            }
            protection ++;
        }
        trap.transform.position = (Vector3)position;
    }
}
