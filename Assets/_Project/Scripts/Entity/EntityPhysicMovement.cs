using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public enum PhysicPriority {
    PLAYER_INPUT = 0, HIGH_PRIORITY = 50
}

public class EntityPhysicMovement : MonoBehaviour {
    [SerializeField] EntityPhysics _entityPhysics;

    [Header("Movements")]
    [SerializeField] float _maxSpeed = 10f;
    [SerializeField] AnimationCurve _acceleration = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] float _accelerationTime = 1f;
    [SerializeField] AnimationCurve _deceleration = AnimationCurve.Linear(1f, 1f, 0f, 0f);
    [SerializeField] float _decelerationTime = 1f;

    Vector2 _orientation = Vector2.zero;
    Force _forceMovement;

    private void Start() {
        Debug.Log(_entityPhysics);
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
}
