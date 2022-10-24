using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityBoss : MonoBehaviour {
    [SerializeField] EntityAbilities _entityAbilities;
    [SerializeField] MenacePointSystemReference _threatSystem;
    [SerializeField] List<BossPhase> _bossPhases;
    [SerializeField] int _currentPhase;
    [SerializeField] float _timeBetweenAttack;
    Timer _timer;
    private void Start() {
        _timer = new Timer(this, _timeBetweenAttack);
        _timer.OnActivate += Attack;
        _timer.Start();
    }
    void Attack() {
        _bossPhases[_currentPhase - 1].GetAnAttack().Activate(_entityAbilities, _threatSystem.Instance.Threatening().transform);
    }

}
