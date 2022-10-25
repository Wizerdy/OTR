using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;
using System.Security.Cryptography;

public class EntityMovement : MonoBehaviour, IEntityAbility {
    private enum State { NONE, ACCELERATING, DECELERATING, TURNING, TURNING_AROUND, DASHING, RESTRAINED }

    public class SpeedModifier {
        float _percentage;
        float _time;
        Coroutine _routine;

        public float Percentage { get => _percentage; set => _percentage = value; }
        public Coroutine Routine => _routine;

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

    [SerializeField] AmplitudeCurve _dash;

    [SerializeField, HideInInspector] BetterEvent _onRun = new BetterEvent();
    [SerializeField, HideInInspector] BetterEvent<Vector2> _onAcceleration = new BetterEvent<Vector2>();
    [SerializeField, HideInInspector] BetterEvent<Vector2> _onDeceleration = new BetterEvent<Vector2>();
    [SerializeField, HideInInspector] BetterEvent<Vector2> _onTurnAround = new BetterEvent<Vector2>();
    [SerializeField, HideInInspector] BetterEvent<Vector2> _onTurn = new BetterEvent<Vector2>();
    [SerializeField, HideInInspector] BetterEvent<Vector2> _onDashStart = new BetterEvent<Vector2>();
    [SerializeField, HideInInspector] BetterEvent _onDashEnd = new BetterEvent();

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

    Coroutine _movement;

    #region Properties

    public bool CanMove {
        get => _cantMoveToken <= 0;
        set {
            if (!value) { _cantMoveToken++; } else { _cantMoveToken = Mathf.Max(--_cantMoveToken, 0); }
        }
    }
    public Rigidbody2D Rigidbody => _rb;
    public bool IsMoving => _speed >= 0.01f;
    public Vector2 Orientation => _orientation;
    public Vector2 Direction => _direction;

    public event UnityAction OnRun { add => _onRun.AddListener(value); remove => _onRun.RemoveListener(value); }
    public event UnityAction<Vector2> OnAcceleration { add => _onAcceleration.AddListener(value); remove => _onAcceleration.RemoveListener(value); }
    public event UnityAction<Vector2> OnDeceleration { add => _onDeceleration.AddListener(value); remove => _onDeceleration.RemoveListener(value); }
    public event UnityAction<Vector2> OnTurnAround { add => _onTurnAround.AddListener(value); remove => _onTurnAround.RemoveListener(value); }
    public event UnityAction<Vector2> OnTurn { add => _onTurn.AddListener(value); remove => _onTurn.RemoveListener(value); }
    public event UnityAction<Vector2> OnDashStart { add => _onDashStart.AddListener(value); remove => _onDashStart.RemoveListener(value); }
    public event UnityAction OnDashEnd { add => _onDashEnd.AddListener(value); remove => _onDashEnd.RemoveListener(value); }

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
        if (_state == State.RESTRAINED) { return; }
        if (_state == State.DASHING) { UpdateDash(); return; }
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

    void UpdateDash() {
        _dash.UpdateTimer(Time.deltaTime);
        float speed = _dash.Evaluate() * _dash.amplitude;

        _rb.velocity = speed * _orientation;

        if (_dash.Percentage >= 1f) {
            StopDash();
        }
    }

    private void StartAcceleration() {
        _acceleration.Reset();
        _acceleration.SetPercentage(_speed / _currentMaxSpeed);
        _state = State.ACCELERATING;
        _onAcceleration.Invoke(_orientation);
    }

    private void StartDecelerating() {
        _deceleration.Reset();
        _deceleration.SetPercentage(1f - _speed / _currentMaxSpeed);
        _state = State.DECELERATING;
        _onDeceleration.Invoke(_orientation);
    }

    private void StartTurn() {
        //_turnTimer = 0f;
        //_acceleration.Reset();
        //StartDecelerating();
        _turnPerc = Mathf.Clamp01(_speed / _currentMaxSpeed);
        _state = State.TURNING;
        _onTurn.Invoke(_orientation);
    }

    private void StartTurnAround() {
        _turnAroundTimer = 0f;
        _acceleration.Reset();
        StartDecelerating();
        _state = State.TURNING_AROUND;
        _onTurnAround.Invoke(_orientation);
    }

    private void SetSpeed(float speed) {
        if (_rb == null) { return; }
        if (_currentMaxSpeed > 0f && speed >= _currentMaxSpeed) { _onRun?.Invoke(); }
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

    #region Movements

    public void Move(Vector2 direction) {
        _direction = direction.normalized;
    }

    public void Stop() {
        Move(Vector2.zero);
        SetSpeed(0f);
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


    struct Movement {
        public Movement(float duration, float speedReference, Vector2 direction = default, AnimationCurve curve = null) {
            _speedReference = speedReference;
            if (curve == null) {
                _curve = new AnimationCurve();
                _curve.AddKey(0, 1);
                _curve.AddKey(1, 1);
            } else {
                _curve = curve;
            }
            _duration = duration;
            if (direction != default) {
                _direction = direction.normalized;
            } else {
                _direction = Vector2.zero;
            }
        }
        public Vector2 _direction;
        public float _speedReference;
        public AnimationCurve _curve;
        public float _duration;
    }

    public void CreateMovement(float duration, float speedReference, Vector2 direction, AnimationCurve curve = null) {
        StopMovement();
        Movement newMovement = new Movement(duration, speedReference, direction, curve);
        _movement = StartCoroutine(NewMovement(newMovement));
    }

    IEnumerator NewMovement(Movement movement) {
        _state = State.RESTRAINED;
        Timer timer = new Timer(this, movement._duration);
        bool finish = false;
        bool constant = false;
        timer.OnActivate += () => finish = true;
        Vector2 directionSave;
        Rigidbody.velocity = Vector3.zero;
        timer.Start();
        if (movement._direction == Vector2.zero) {
            directionSave = Orientation.normalized;
        } else {
            directionSave = movement._direction;
            constant = true;
        }

        while (!finish) {
            if (constant) {
                Rigidbody.velocity = directionSave * movement._speedReference * movement._curve.Evaluate(Mathf.InverseLerp(0,timer.Duration,timer.CurrentDuration));
            } else {
                if (_direction.normalized != Vector2.zero) {
                    directionSave = _direction.normalized;
                    Rigidbody.velocity = _direction.normalized * movement._speedReference * movement._curve.Evaluate(Mathf.InverseLerp(0, timer.Duration, timer.CurrentDuration));
                } else {
                    Rigidbody.velocity = directionSave * movement._speedReference * movement._curve.Evaluate(Mathf.InverseLerp(0, timer.Duration, timer.CurrentDuration));
                }
            }
            yield return null;
        }
        _movement = null;
        _state = State.DECELERATING;
    }

    public void StopMovement() {
        if (_movement != null) {
            StopCoroutine(_movement);
        }
        _rb.velocity = Vector2.zero;
    }

    #endregion

    #region Dash

    public void Dash(Vector2 direction) {
        if (!CanMove) { return; }

        CanMove = false;
        _orientation = direction;
        _dash.Reset();
        _state = State.DASHING;
        _speed = 0f;

        _onDashStart.Invoke(direction);
    }

    private void StopDash() {
        if (_state != State.DASHING) { return; }
        CanMove = true;
        _state = State.DECELERATING;
        _speed = 0f;

        _onDashEnd.Invoke();
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

    public SpeedModifier SetSlow(SpeedModifier modifier, float percentage) {
        modifier.Percentage = percentage;
        RefreshSlows();
        return modifier;
    }

    public void RemoveSlow(SpeedModifier modifier) {
        if (modifier.Routine != null) { StopCoroutine(modifier.Routine); }
        _slows.Remove(modifier);

        if (_currentSlow == modifier) {
            RefreshSlows();
        }
    }

    private void RefreshSlows() {
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

    #endregion
}
