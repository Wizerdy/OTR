using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class CrawlerEntity : MonoBehaviour {
    enum State { NONE, PATROLLING, CHASING, ATTACKING }

    [SerializeField] EntityMovement _entityMovements;
    [SerializeField] EntityOrientation _entityOrientation;
    [SerializeField] EntityAbilities _entityAbilities;
    [SerializeField] EntityAttacks _entityAttacks;

    [SerializeField] float _timeBewteenPatrol = 1f;
    //[SerializeField] float _attackTime = 1f;

    [SerializeField, HideInInspector] BetterEvent<RaycastHit2D> _onView = new BetterEvent<RaycastHit2D>();

    #region Events

    public event UnityAction<RaycastHit2D> OnView { add => _onView.AddListener(value); remove => _onView.RemoveListener(value); }

    #endregion

    Transform _chaseTarget;
    State state = State.NONE;
    Coroutine _movementRoutine;

    private void Start() {
        ChangeState(State.PATROLLING);
    }

    public void Chase(Transform obj) {
        _chaseTarget = obj.transform;
        ChangeState(State.CHASING);
    }

    public void Partol() {
        ChangeState(State.PATROLLING);
    }

    public void Attack(GameObject target, Vector2 direction) {
        if (State.ATTACKING == state) { return; }
        ChangeState(State.ATTACKING);
        _entityOrientation.LookAt(direction);
        StartCoroutine(AttackRoutine(target, direction));
    }

    private void ChangeState(State newState) {
        if (newState == state) { return; }

        if (_movementRoutine != null) { StopCoroutine(_movementRoutine); }

        state = newState;
        if (newState != State.CHASING) { _chaseTarget = null; }
        IEnumerator routine;
        switch (newState) {
            case State.PATROLLING:
                routine = PatrolRoutine();
                break;
            case State.CHASING:
                routine = ChaseRoutine();
                break;
            default:
                routine = null;
                break;
        }
        if (routine == null) { return; }
        _movementRoutine = StartCoroutine(routine);
    }

    IEnumerator ChaseRoutine() {
        Vector2 direction;
        while (state == State.CHASING) {
            if (_chaseTarget == null) {
                ChangeState(State.PATROLLING);
                yield break;
            }
            direction = (_chaseTarget?.Position2D() - transform.Position2D()) ?? Vector2.zero;
            MoveToward(direction);
            yield return null;
        }
    }

    IEnumerator PatrolRoutine() {
        Vector2 direction;
        WaitForSeconds timer = new WaitForSeconds(_timeBewteenPatrol);
        //WaitForSeconds halfTimer = new WaitForSeconds(_timeBewteenPatrol/2f);
        while (state == State.PATROLLING) {
            direction = Random.onUnitSphere;
            MoveToward(direction);
            yield return timer;
            _entityMovements?.Move(Vector2.zero);
            yield return timer;
        }
    }

    IEnumerator AttackRoutine(GameObject target, Vector2 direction) {
        Debug.Log("ATTACK MONKEY !");
        yield return _entityAttacks.Use("Test", target, direction);
        ChangeState(State.PATROLLING);
        //yield return new WaitForSeconds(_attackTime);
    }

    private void MoveToward(Vector2 direction) {
        direction.Normalize();
        _entityMovements?.Move(direction);
        _entityOrientation?.LookAt(direction);
    }
}
