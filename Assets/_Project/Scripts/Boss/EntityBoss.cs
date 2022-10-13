using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBoss : MonoBehaviour {
    [SerializeField] EntityAbilities _entityAbilities;
    [SerializeField] List<EntityThreatPoint> _threatPoints;
    [SerializeField] List<BossPhase> _bossPhases;
    [SerializeField] int _currentPhase;
    Timer _timer;
    private void Start() {
        _timer = new Timer(this, 10);
        _timer.OnActivate += Attack;
        _timer.Start();
    }
    void Attack() {
        _bossPhases[_currentPhase - 1].GetAnAttack().Activate(_entityAbilities, GetMainThreat());
    }

    Transform GetMainThreat() {
        float maxPoint = 0;
        Transform transform = null;
        for (int i = 0; i < _threatPoints.Count; i++) {
            if (_threatPoints[i].CurrentValue > maxPoint) {
                maxPoint = _threatPoints[i].CurrentValue;
                transform = _threatPoints[i].transform;
            }
        }
        return transform;
    }
}
