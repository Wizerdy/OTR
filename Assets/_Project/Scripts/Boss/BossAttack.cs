using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sloot;
using ToolsBoxEngine.BetterEvents;
using UnityEngine.Events;

public abstract class  BossAttack : MonoBehaviour {
    [Header("Boss Attack General :")]
    [SerializeField] protected float _weight;
    [SerializeField] float _endDuration;
    [SerializeField, HideInInspector] BetterEvent _finished;
    public float Weight => _weight;

    public event UnityAction Finished { add => _finished.AddListener(value); remove => _finished.RemoveListener(value); }

    public abstract void Activate(EntityAbilities ea, Transform target);

    public abstract void Activate(EntityAbilities ea, Transform[] targets);

    protected void End() {
        StartCoroutine(Wait());
    }

    IEnumerator Wait() {
        yield return new WaitForSeconds(_endDuration);
        _finished?.Invoke();
    }

}
