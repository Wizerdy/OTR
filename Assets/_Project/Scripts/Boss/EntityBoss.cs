using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;
using UnityEngine.Events;

public class EntityBoss : MonoBehaviour, IEntityAbility {
    [SerializeField] EntityAbilities _entityAbilities;
    [SerializeField] MenacePointSystemReference _threatSystem;
    [SerializeField] List<BossPhase> _bossPhases;
    [SerializeField] int _currentPhase;
    [SerializeField] BossAttack _currentAttack;
    [SerializeField] BossAttack _nextAttack;
    [SerializeField] BetterEvent _newPhase;
    [SerializeField] Transform _center;
    [SerializeField] SpriteRenderer _sr;
    [SerializeField] float _delayAttackNewPhase;

    public event UnityAction NewPhase { add => _newPhase.AddListener(value); remove => _newPhase.RemoveListener(value); }
    Timer _timer;
    private void Start() {
        _sr = _entityAbilities.GetComponentInChildren<SpriteRenderer>();
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
        _newPhase?.Invoke();
        TeleportCenter();
        ChangeColor();
        _entityAbilities.Get<EntityPhysics>().Purge();
        _currentAttack.StopAllCoroutines();
        StartCoroutine(Tools.Delay(() => Attack(), _delayAttackNewPhase)); 
    }

    void TeleportCenter() {
        _entityAbilities.transform.position = _center.position;
    }

    void ChangeColor() {
        Color NewColor = new (1/255f * Random.Range(0, 255), 1 / 255f * Random.Range(0, 255), 1 / 255f * Random.Range(0, 255), 1);
        _sr.color = NewColor;
    }
}
