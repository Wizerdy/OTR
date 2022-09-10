using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class CrawlerEntity : MonoBehaviour {
    enum State { NONE, PATROLLING, CHASE }

    [SerializeField] EntityMovement _entityMovements;
    [SerializeField] EntityOrientation _entityOrientation;

    [SerializeField] float _timeBewteenPatrol = 1f;
    [SerializeField] float _attackTime = 1f;

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
        ChangeState(State.CHASE);
    }

    public void Partol() {
        ChangeState(State.PATROLLING);
    }

    private void ChangeState(State newState) {
        if (newState == state) { return; }

        if (_movementRoutine != null) { StopCoroutine(_movementRoutine); }

        state = newState;
        if (newState != State.CHASE) { _chaseTarget = null; }
        IEnumerator routine;
        switch (newState) {
            default:
            case State.PATROLLING:
                routine = PatrolRoutine();
                break;
            case State.CHASE:
                routine = ChaseRoutine();
                break;
        }
        _movementRoutine = StartCoroutine(routine);
    }

    IEnumerator ChaseRoutine() {
        Vector2 direction;
        while (state == State.CHASE) {
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

    IEnumerator AttackRoutine(Vector2 direction) {
        yield return new WaitForSeconds(_attackTime);
    }

    private void MoveToward(Vector2 direction) {
        direction.Normalize();
        _entityMovements?.Move(direction);
        _entityOrientation?.LookAt(direction);
    }
}
