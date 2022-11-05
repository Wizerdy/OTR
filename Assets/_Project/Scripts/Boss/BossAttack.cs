using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sloot;
using ToolsBoxEngine.BetterEvents;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using static UnityEngine.GraphicsBuffer;
using Unity.VisualScripting;

public abstract class BossAttack : MonoBehaviour {
    [Header("Boss Attack General :")]
    [SerializeField] protected float _weight;
    [SerializeField] protected float _endDuration;
    [SerializeField] protected int _damages;
    [SerializeField, HideInInspector] BetterEvent _finished;
    BetterEvent _begining;
    BetterEvent _middle;
    BetterEvent _end;
    bool _isActive;
    public float Weight => _weight;
    public bool IsActive => _isActive;

    public event UnityAction Begining { add => _begining.AddListener(value); remove => _begining.RemoveListener(value); }
    public event UnityAction Middle { add => _middle.AddListener(value); remove => _middle.RemoveListener(value); }
    public event UnityAction End { add => _end.AddListener(value); remove => _end.RemoveListener(value); }
    public event UnityAction Finished { add => _finished.AddListener(value); remove => _finished.RemoveListener(value); }

    public void Activate(EntityAbilities ea, Transform target) {
        StartCoroutine(Launch(ea, target));
    }

    IEnumerator Launch(EntityAbilities ea, Transform target) {
        _isActive = true;
        yield return Attack(ea, target);
        yield return new WaitForSeconds(_endDuration);
        _isActive = false;
        _finished?.Invoke();
    }

    protected virtual IEnumerator AttackBegins(EntityAbilities ea, Transform target) {
        yield break;
    }
    protected abstract IEnumerator AttackMiddle(EntityAbilities ea, Transform target);
    protected virtual IEnumerator AttackEnds(EntityAbilities ea, Transform target) {
        yield break;
    }

    protected virtual IEnumerator Attack(EntityAbilities ea, Transform target) {
        _begining?.Invoke();
        yield return AttackBegins(ea, target);
        _middle?.Invoke();
        yield return AttackMiddle(ea, target);
        _end?.Invoke();
        yield return AttackEnds(ea, target);
    }
}
