using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;
using UnityEngine.Events;

public class EntityBoss : MonoBehaviour, IEntityAbility {
    [SerializeField] EntityAbilities _entityAbilities;
    [SerializeField] MenacePointSystemReference _threatSystem;
    [SerializeField] Transform _center;
    [SerializeField] int _currentPhase;
    [SerializeField] List<BossPhase> _bossPhases;
    [SerializeField] BetterEvent _newPhase;
    [SerializeField] GameObject _phases;
    [SerializeField] float _delayAttackNewPhase;
    BossAttack _currentAttack;
    BossAttack _nextAttack;
     Animator _animator;

    public event UnityAction NewPhase { add => _newPhase.AddListener(value); remove => _newPhase.RemoveListener(value); }
    Timer _timer;
    private void Start() {
        if(_phases != null) {
            for (int i = 0; i < _phases.transform.childCount; i++) {
                BossPhase phase = _phases.transform.GetChild(i).GetComponent<BossPhase>();
                if (phase != null) {
                    if(!_bossPhases.Contains(phase))
                    _bossPhases.Add(phase);
                }
            }
        }
        for (int i = 0; i < _bossPhases.Count; i++) {
            if (_bossPhases[i] == null) {
                _bossPhases.RemoveAt(i);
                i--;
            }
        }
        _animator = _entityAbilities.GetComponentInChildren<Animator>();
        StartCoroutine(Tools.DelayOneFrame(() => Attack()));
    }

    void Attack() {
        if (_currentAttack != null)
            _currentAttack.Finished -= Attack;
        if (_nextAttack != null) {
            _currentAttack = _nextAttack;
            _nextAttack = null;
        } else {
            _currentAttack = _bossPhases[_currentPhase].GetAnAttack();
            _currentAttack.Finished += Attack;
            _currentAttack.Activate(_entityAbilities, _threatSystem.Instance.Threatening().transform);
        }
    }

    public void PhasePlusPlus() {
        _currentPhase++;
        TeleportCenter();
        _entityAbilities.Get<EntityPhysics>().Purge();
        _currentAttack.StopAllCoroutines();
        StartCoroutine(Tools.Delay(() => Attack(), _delayAttackNewPhase)); 
        _newPhase?.Invoke();
    }

    void TeleportCenter() {
        _entityAbilities.transform.position = _center.position;
    }

    public void PlayAnimationTrigger(string animation) {
        _animator.SetTrigger(animation);
    }

    public void PlayAnimationBool(string animation, bool value) {
        _animator.SetBool(animation, value);
    }

    public void PlayAnimationTrigger() {
        Debug.Log("oui");
    }
}
