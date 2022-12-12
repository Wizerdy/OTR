using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine.BetterEvents;

public class BossOneForAll : BossAttack {
    [SerializeField] float _timeBeforeActivation = 2f;
    [SerializeField] Vector2 _distMinMax = new Vector2(2,10);
    [SerializeField] Vector2 _damagesVector = new Vector2(0, 10);
    [SerializeField] ForAllerParasite _forAllerPrefab;
    [SerializeField] BetterEvent _onOneForAllStart = new BetterEvent();
    public event UnityAction OnOneForAllStart { add => _onOneForAllStart += value; remove => _onOneForAllStart -= value; }
    EntityAbilities[] _playersEA;
    ForAllerParasite[] _forAllersParasite;

    protected override IEnumerator AttackBegins(EntityAbilities ea, Transform target) {
        PlayerEntity[] players = FindObjectsOfType<PlayerEntity>();
        _playersEA = new EntityAbilities[players.Length];
        for (int i = 0; i < _playersEA.Length; i++) {
            _playersEA[i] = players[i].transform.parent.GetComponent<EntityAbilities>();
        }
        _damagesVector.x = _damages;
        _forAllersParasite = new ForAllerParasite[_playersEA.Length];
        for (int i = 0; i < _playersEA.Length; i++) {
            _forAllersParasite[i] = Instantiate(_forAllerPrefab, _playersEA[i].transform)
                .ChangeHost(_playersEA[i].GetComponent<IHealth>())
                .ChangeDamages(_damagesVector)
                .ChangeDist(_distMinMax)
                .ChangeTimeBeforeActivation(_timeBeforeActivation);
            _forAllersParasite[i].transform.localPosition = Vector3.zero;
        }
        for (int i = 0; i < _forAllersParasite.Length; i++) {
            _forAllersParasite[i].ChangeParasite(_forAllersParasite.ToList());
        }
        _entityBoss.SetAnimationTrigger("OneForAll-ing");
        yield break;
    }
    protected override IEnumerator AttackMiddle(EntityAbilities ea, Transform target) {
        _onOneForAllStart?.Invoke();
        yield return new WaitForSeconds(_timeBeforeActivation);
    }
}
