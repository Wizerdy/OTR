using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class EntityBoss : MonoBehaviour {
    [SerializeField] EntityAbilities _entityAbilities;
    [SerializeField] MenacePointSystemReference _threatSystem;
    [SerializeField] List<BossPhase> _bossPhases;
    [SerializeField] int _currentPhase;
    [SerializeField] BossAttack _currentAttack;
    Timer _timer;
    private void Start() {
        StartCoroutine(Tools.DelayOneFrame(() => Attack()));
    }
    void Attack() {
        if(_currentAttack != null)
        _currentAttack.Finished -= Attack;
        _currentAttack = _bossPhases[_currentPhase].GetAnAttack();
        _currentAttack.Finished += Attack;
        _currentAttack.Activate(_entityAbilities, _threatSystem.Instance.Threatening().transform);
    }
}
