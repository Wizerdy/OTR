using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public enum PhysicPriority {
    PLAYER_INPUT = 0, DASH = 5, HIGH_PRIORITY = 50
}

public class EntityPhysicMovement : MonoBehaviour {
    [SerializeField] EntityPhysics _entityPhysics;

    [Header("Movements")]
    [SerializeField] float _maxSpeed = 10f;
    [SerializeField] AnimationCurve _acceleration = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] float _accelerationTime = 1f;
    [SerializeField] AnimationCurve _deceleration = AnimationCurve.Linear(0f, 1f, 1f, 0f);
    [SerializeField] float _decelerationTime = 1f;

    Token _cantMoveToken = new Token();
    StackableFunc<float> _speedModifier = new StackableFunc<float>();

    Vector2 _orientation = Vector2.zero;
    Force _forceMovement;

    public bool CanMove { get => !_cantMoveToken.HasToken; set => _cantMoveToken.AddToken(!value); }

    private void Reset() {
        _entityPhysics = GetComponent<EntityPhysics>();
    }

    private void Start() {
        _cantMoveToken.OnFill += _DontMove;

        _forceMovement = _entityPhysics.Add(
            _maxSpeed, Vector2.zero, 1f,
            Force.ForceMode.INPUT,
            _acceleration, _accelerationTime,
            _deceleration, _decelerationTime,
            (int)PhysicPriority.PLAYER_INPUT);
    }

    public void Bump() {
        _entityPhysics.Add(20f, Vector2.right, 0.7f, Force.ForceMode.TIMED, AnimationCurve.Constant(0f, 1f, 1f), 0f, _deceleration, 5f, (int)PhysicPriority.HIGH_PRIORITY);
    }

    public void Move(Vector2 direction) {
        if (!CanMove) { return; }
        MoveDirection(direction);
    }

    public void Stop() {
        _forceMovement.Reset();
        _DontMove();
    }

    private void MoveDirection(Vector2 direction) {
        if (_forceMovement == null) { return; }
        if (_forceMovement.State == Force.ForceState.DECELERATION && direction != Vector2.zero) {
            _forceMovement.ChangeState(Force.ForceState.ACCELERATION);
        } else if (_forceMovement.State != Force.ForceState.DECELERATION && direction == Vector2.zero) {
            _forceMovement.ChangeState(Force.ForceState.DECELERATION);
        }
        
        _forceMovement.Direction = direction != Vector2.zero ? direction : _orientation;

        if (direction != Vector2.zero) {
            _orientation = direction;
        }
    }

    private void _DontMove() {
        MoveDirection(Vector2.zero);
    }

    private void AddSpeedModifier(float modifier) {
        _speedModifier.Add(_speedModifier);
    }

    float _SpeedModifier(float source, float modifier) {
        return source * modifier;
    }
}
