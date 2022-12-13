using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;
using System.Collections;

public class EntityBoss : MonoBehaviour, IEntityAbility {
    [SerializeField] EntityAbilities _entityAbilities;
    [SerializeField] MenacePointSystemReference _threatSystem;
    [SerializeField] Transform _center;
    [SerializeField] int _currentPhase;
    [SerializeField] List<BossPhase> _bossPhases;
    [SerializeField] BetterEvent _newPhase;
    [SerializeField] GameObject _phases;
    [SerializeField] float _delayAttackNewPhase;
    [SerializeField] BossAttack _currentAttack;
    BossAttack _nextAttack;
    Animator _animator;
    SpriteRenderer _spriteRenderer;
    bool died = false;

    public event UnityAction NewPhase { add => _newPhase.AddListener(value); remove => _newPhase.RemoveListener(value); }
    private void Start() {
        _animator = _entityAbilities.GetComponentInChildren<Animator>();
        _spriteRenderer = _entityAbilities.GetComponentInChildren<SpriteRenderer>();
        if (_phases != null) {
            for (int i = 0; i < _phases.transform.childCount; i++) {
                BossPhase phase = _phases.transform.GetChild(i).GetComponent<BossPhase>();
                if (!_bossPhases.Contains(phase))
                    _bossPhases.Add(phase);

            }
        }
        for (int i = 0; i < _bossPhases.Count; i++) {
            if (_bossPhases[i] == null) {
                _bossPhases.RemoveAt(i);
                i--;
            }
        }
        StartCoroutine(Tools.DelayOneFrame(() => Attack()));
    }

    void Attack() {
        if(died) { return; }
        FlipRight(true);
        if (_currentAttack != null)
            _currentAttack.Finished -= Attack;
        if (_nextAttack != null) {
            _currentAttack = _nextAttack;
            _nextAttack = null;
        } else {
            _currentAttack = _bossPhases[_currentPhase].GetAnAttack();
            _currentAttack.Finished += Attack;
            _currentAttack.Activate(_entityAbilities, _threatSystem.Instance.Threatening()?.transform);
        }
    }

    public void PhasePlusPlus() {
        if (died) { return; }
        _newPhase?.Invoke();
        _entityAbilities.Get<EntityPhysics>().Purge();
        _currentAttack.Disable();
        StartCoroutine(TeleportCenter());
        _currentPhase++;
        
       // _animator.SetTrigger("NewPhase");
    }

    IEnumerator TeleportCenter() {
        if (died) { yield break; }
        foreach (AnimatorControllerParameter parameter in _animator.parameters) {
            if (parameter.type == AnimatorControllerParameterType.Bool)
                _animator.SetBool(parameter.name, false);
            if(parameter.type == AnimatorControllerParameterType.Trigger)
                _animator.ResetTrigger(parameter.name);
        }
        Health health = _entityAbilities.GetComponentInChildren<Health>();
        health.CanTakeDamage = false;
        SetAnimationTrigger("StartTp");
        while (!GetAnimationBool("CanTp1")) {
            yield return null;
        }
        _entityAbilities.transform.position = _center.position;
        SetAnimationTrigger("EndTp");
        while (!GetAnimationBool("CanTp2")) {
            yield return null;
        }
        SetAnimationBool("CanTp1", false);
        SetAnimationBool("CanTp2", false);
        health.CanTakeDamage = true;
        yield return new WaitForSeconds(_delayAttackNewPhase);
        Attack();
    }

    public void SetAnimationTrigger(string animation) {
        _animator.SetTrigger(animation);
    }

    public void SetAnimationBool(string animation, bool value) {
        _animator.SetBool(animation, value);
    }

    public void SetAnimationBoolTrue(string animation) {
        _animator.SetBool(animation, true);
    }

    public void SetAnimationBoolFalse(string animation) {
        _animator.SetBool(animation, false);
    }

    public bool GetAnimationBool(string animation) {
        return _animator.GetBool(animation);
    }

    public void FlipRight(bool intoRight) {
        if (intoRight) {
            _spriteRenderer.transform.localScale = Vector3.one;
        } else {
            _spriteRenderer.transform.localScale = new Vector3(-1, 1, 1);
        }
    }

    public void Die() {
        died = true;
        StopAllCoroutines();
        _currentAttack.StopAllCoroutines();
        _animator.SetTrigger("Died");
    }
}
