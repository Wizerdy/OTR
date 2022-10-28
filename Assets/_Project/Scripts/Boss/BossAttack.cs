using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sloot;
using ToolsBoxEngine.BetterEvents;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public abstract class  BossAttack : MonoBehaviour {
    [Header("Boss Attack General :")]
    [SerializeField] protected float _weight;
    [SerializeField] float _endDuration;
    [SerializeField, HideInInspector] BetterEvent _finished;
     bool _isActive;
    public float Weight => _weight;
    public bool IsActive => _isActive;

    public event UnityAction Finished { add => _finished.AddListener(value); remove => _finished.RemoveListener(value); }

    public void Activate(EntityAbilities ea, Transform target) {
        _isActive = true;
        StartCoroutine(Attack(ea,target));
    }

    protected abstract IEnumerator Attack(EntityAbilities ea, Transform target);
    protected virtual IEnumerator EndAttack() {
        yield break;
    }

    protected IEnumerator Disactivate() {
        yield return StartCoroutine(EndAttack());
        yield return StartCoroutine(Wait());
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(_endDuration);
        _isActive = false;
        _finished?.Invoke();
    }
}
