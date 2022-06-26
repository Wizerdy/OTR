using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class EntityMovement : MonoBehaviour {
    private enum State { NONE, ACCELERATING, DECELERATING, TURNING, TURNING_AROUND }

    public class SpeedModifier {
        float _percentage;
        float _time;
        Coroutine _routine;

        public float Percentage => _percentage;

        public SpeedModifier(float percentage, float time, Coroutine routine = null) {
            _percentage = percentage;
            _time = time;
            _routine = routine;
        }

        public void SetCoroutine(Coroutine routine) {
            _routine = routine;
        }
    }

    [SerializeField] Rigidbody2D _rb = null;
    [SerializeField] float _maxSpeed = 5f;
    [SerializeField] AmplitudeCurve _acceleration;
    [SerializeField] AmplitudeCurve _deceleration;
    //[SerializeField] float _turnFactor;
    [SerializeField] AnimationCurve _turnFactor;
    [SerializeField, Range(0, 360f)] float _turnAroundAngle = 181f;

    public Tools.BasicDelegate OnRun;
    public Tools.BasicDelegate<Vector2> OnAcceleration;
    public Tools.BasicDelegate<Vector2> OnDeceleration;
    public Tools.BasicDelegate<Vector2> OnTurnAround;
    public Tools.BasicDelegate<Vector2> OnTurn;

    float _currentMaxSpeed = 5f;
    Vector2 _direction = Vector2.zero;
    Vector2 _orientation = Vector2.zero;
    Vector2 _lastDirection = Vector2.zero;
    State _state = State.DECELERATING;

    float _speed = 0f;
    float _turnAroundRadian = 0f;
    float _turnAroundTimer = 0f;
    float _turnPerc = 0f;

    int _cantMoveToken = 0;

    List<SpeedModifier> _slows = new List<SpeedModifier>();
    SpeedModifier _currentSlow = null;

    SpeedModifier _moveToSlow = null;

    #region Properties

    public bool CanMove {
        get => _cantMoveToken <= 0;
        set {
            if (!value) { _cantMoveToken++; } else { _cantMoveToken = Mathf.Max(--_cantMoveToken, 0); }
        }
    }
    public bool IsMoving => _speed >= 0.01f;
    public Vector2 Orientation => _orientation;
    public Vector2 Direction => _direction;

    #endregion

    #region UnityCallbacks

    private void Reset() {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void Awake() {
        _turnAroundRadian = Mathf.Cos(_turnAroundAngle / 2f);
        if (_turnAroundAngle == 360f) { _turnAroundRadian = -10f; }
        _currentMaxSpeed = _maxSpeed;
    }

    private void FixedUpdate() {
        UpdateMove();
    }

    private void UpdateMove() {
        if (!CanMove) { return; }
        if (_currentMaxSpeed <= 0f) { SetSpeed(0f); return; }

        if (_direction != Vector2.zero) {
            if ((!IsMoving && _state == State.DECELERATING) || (_direction == _orientation && _state == State.DECELERATING)) {
                StartAcceleration();
            } else if (Vector2.Dot(_direction, _orientation) < _turnAroundRadian) {
                if (_state != State.TURNING_AROUND) {
                    StartTurnAround();
                }
            } else if (_direction != _lastDirection && _direction != _orientation) {
                StartTurn();
            }
        } else {
            if (_lastDirection != Vector2.zero) {
                StartDecelerating();
            }
        }

        bool changeDirection;
        float speed = ComputeSpeed(_state, out changeDirection);
        SetSpeed(speed);

        if (changeDirection) { _orientation = _direction; }

        _lastDirection = _direction;
    }

    #endregion

    #region Movements

    public void Move(Vector2 direction) {
        _direction = direction.normalized;
    }

    public Coroutine MoveTo(Vector2 position, float speedFactor = 1f) {
        return StartCoroutine(IMoveTo(position, speedFactor));
    }

    IEnumerator IMoveTo(Vector2 position, float speedFactor) {
        _moveToSlow = Slow(speedFactor, 50f);
        while (transform.Position2D() != position) {
            Vector2 direction = (position - transform.Position2D());
            if (direction.sqrMagnitude < 1f) {
                Move(Vector2.zero);
                break;
            }
            Move(direction.normalized);
            yield return new WaitForEndOfFrame();
        }
        RemoveSlow(_moveToSlow);
    }

    private void StartAcceleration() {
        _acceleration.Reset();
        _acceleration.SetPercentage(_speed / _currentMaxSpeed);
        _state = State.ACCELERATING;
        OnAcceleration?.Invoke(_orientation);
    }

    private void StartDecelerating() {
        _deceleration.Reset();
        _deceleration.SetPercentage(1f - _speed / _currentMaxSpeed);
        _state = State.DECELERATING;
        OnDeceleration?.Invoke(_orientation);
    }

    private void StartTurn() {
        //_turnTimer = 0f;
        //_acceleration.Reset();
        //StartDecelerating();
        _turnPerc = Mathf.Clamp01(_speed / _currentMaxSpeed);
        _state = State.TURNING;
        OnTurn?.Invoke(_orientation);
    }

    private void StartTurnAround() {
        _turnAroundTimer = 0f;
        _acceleration.Reset();
        StartDecelerating();
        _state = State.TURNING_AROUND;
        OnTurnAround?.Invoke(_orientation);
    }

    private void SetSpeed(float speed) {
        if (_rb == null) { return; }
        if (_currentMaxSpeed > 0f && speed >= _currentMaxSpeed) { OnRun?.Invoke(); }
        _speed = speed;
        _rb.velocity = speed * _orientation;
    }

    private float ComputeSpeed(State state, out bool changeDirection) {
        changeDirection = false;
        switch (state) {
            case State.NONE:
                break;
            case State.ACCELERATING:
                _acceleration.UpdateTimer(Time.deltaTime);
                changeDirection = true;
                return _acceleration.Evaluate() * _currentMaxSpeed;
            case State.DECELERATING:
                _deceleration.UpdateTimer(Time.deltaTime);
                changeDirection = false;
                return _deceleration.Evaluate() * _currentMaxSpeed;
            case State.TURNING_AROUND:
                _turnAroundTimer += Time.deltaTime;
                if (_turnAroundTimer < _deceleration.duration) {
                    return ComputeSpeed(State.DECELERATING, out changeDirection);
                } else if ((_turnAroundTimer - _deceleration.duration) < _acceleration.duration) {
                    return ComputeSpeed(State.ACCELERATING, out changeDirection);
                } else {
                    _state = State.ACCELERATING;
                }
                break;
            case State.TURNING:
                changeDirection = true;
                return _turnFactor.Evaluate(_turnPerc) * _currentMaxSpeed;
        }
        return 0f;
    }

    #endregion

    #region Slow

    public SpeedModifier Slow(float percentage, float time) {
        SpeedModifier speedModifier = new SpeedModifier(percentage, time);
        Coroutine routine = null;
        if (time > 0) {
            routine = StartCoroutine(Tools.Delay(() => RemoveSlow(speedModifier), time));
        }
        speedModifier.SetCoroutine(routine);
        _slows.Add(speedModifier);
        if (_currentSlow == null || percentage < _currentSlow.Percentage) {
            _currentMaxSpeed = _maxSpeed * percentage;
            _currentSlow = speedModifier;
        }
        return speedModifier;
    }

    public void RemoveSlow(SpeedModifier modifier) {
        _slows.Remove(modifier);

        if (_currentSlow == modifier) {
            _currentSlow = null;
            if (_slows.Count > 0) {
                if (_slows.Count == 1) {
                    _currentSlow = _slows[0];
                } else {
                    for (int i = 0; i < _slows.Count; i++) {
                        if (_slows[i].Percentage < (_currentSlow?.Percentage ?? 1f)) {
                            _currentSlow = _slows[i];
                        }
                    }
                }
            }

            _currentMaxSpeed = _maxSpeed * (_currentSlow?.Percentage ?? 1f);
        }
    }

    #endregion
}
