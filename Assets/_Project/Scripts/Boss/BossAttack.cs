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
    [SerializeField] bool _isActive;
    [SerializeField, HideInInspector] BetterEvent _finished;
    public float Weight => _weight;

    public event UnityAction Finished { add => _finished.AddListener(value); remove => _finished.RemoveListener(value); }

    public void Activate(EntityAbilities ea, Transform target) {
        _isActive = true;
        StartCoroutine(Attack(ea,target));
    }

    protected abstract IEnumerator Attack(EntityAbilities ea, Transform target);

    protected IEnumerator EndAttack() {
        yield return StartCoroutine(Wait());
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(_endDuration);
        _isActive = false;
        _finished?.Invoke();
    }
}
