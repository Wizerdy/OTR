using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBoss : MonoBehaviour {
    [SerializeField] EntityAbilities _entityAbilities;
    [SerializeField] MenacePointSystemReference _threatSystem;
    [SerializeField] List<BossPhase> _bossPhases;
    [SerializeField] int _currentPhase;
    [SerializeField] BossAttack _currentAttack;
    Timer _timer;
    private void Start() {
        Attack();
    }
    void Attack() {
        if(_currentAttack != null)
        _currentAttack.Finished -= Attack;

        _currentAttack = _bossPhases[_currentPhase - 1].GetAnAttack();
        _currentAttack.Finished += Attack;
        _currentAttack.Activate(_entityAbilities, _threatSystem.Instance.Threatening().transform);
    }

}
