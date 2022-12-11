using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class BossTrap : BossAttack {
    [Header("Prog")]
    [SerializeField] Trap _trapPrefab;
    [Header("GD")]
    [SerializeField] float _minimumDistBetween;
    [SerializeField] float _minimumDistFromSide;
    [SerializeField] float _trapNumber;
    [SerializeField] float _visibleTime;
    [SerializeField] float _effectivityTime;
    [SerializeField] float _tick;


    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        _entityBoss.SetAnimationTrigger("Trapping");
        while (!_entityBoss.GetAnimationBool("CanTrap")) {
            yield return null;
        }
        Physics2D.queriesHitTriggers = true;
        for (int i = 0; i < _trapNumber; i++) {
            PutTrap();
        }
        Physics2D.queriesHitTriggers = false;
        _entityBoss.SetAnimationBool("CanTrap", false);
        yield break;
    }

    void PutTrap() {
        Trap trap = Instantiate(_trapPrefab).ChangeVisibility(_visibleTime).ChangeDamages(_damages).ChangeTick(_tick).ChangeEffectivity(_effectivityTime);
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
                if (collider.gameObject.GetComponent<Trap>() != null) {
                    placed = false;
                }
            }
            protection++;
        }
        trap.transform.position = position;
    }
}
