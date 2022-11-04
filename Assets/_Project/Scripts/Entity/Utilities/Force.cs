using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine.BetterEvents;

[System.Serializable]
public class Force {
    public enum ForceMode {
        TIMED, INPUT
    }

    public enum ForceState {
        ACCELERATION, STAGNATION, DECELERATION
    }

    [SerializeField] float _strength = 5f;
    [SerializeField] Vector2 _direction = Vector2.zero;
    [SerializeField, Range(0f, 1f)] float _weight = 1f;
    [SerializeField] AnimationCurve _start = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] float _startDuration = 1f;
    [SerializeField] AnimationCurve _end = AnimationCurve.Linear(1f, 1f, 0f, 0f);
    [SerializeField] float _endDuration = 1f;
    [SerializeField] ForceMode _mode = ForceMode.TIMED;

    [SerializeField] BetterEvent<Force> _onStart = new BetterEvent<Force>();
    [SerializeField] BetterEvent<Force, ForceState> _onChangeState = new BetterEvent<Force, ForceState>();
    [SerializeField] BetterEvent<Force> _onEnd = new BetterEvent<Force>();

    bool _ignored = false;
    float _currentPercent = 0;
    ForceState _state = ForceState.ACCELERATION;

    #region Properties

    public float Strength { get => _strength; set => _strength = value; }
    public Vector2 Direction { get => _direction; set => _direction = value; }
    public float Weight { get => _weight; set => _weight = value; }
    public AnimationCurve Start => _start;
    public AnimationCurve End => _end;
    public float StartDuration => _startDuration;
    public float EndDuration => _endDuration;
    public float Duration => _startDuration + _endDuration;
    public ForceMode Mode => _mode;
    public ForceState State => _state;

    public bool Ignored { get => _ignored; set => _ignored = value; }
    public bool HasEnded { get => _state == ForceState.DECELERATION && _currentPercent <= 0; }

    #endregion

    #region Events

    public event UnityAction<Force> OnStart { add => _onStart += value; remove => _onStart -= value; }
    public event UnityAction<Force, ForceState> OnChangeState { add => _onChangeState += value; remove => _onChangeState -= value; }
    public event UnityAction<Force> OnEnd { add => _onEnd += value; remove => _onEnd -= value; }

    #endregion

    public Force(Force force) :
        this(force._strength, force._direction, force._weight, force._mode, force._start, force._startDuration, force._end, force._endDuration) {

    }

    public Force(float strength, Vector2 direction, float weight = 1f,
        ForceMode mode = ForceMode.TIMED,
        AnimationCurve start = null, float startTime = 1f,
        AnimationCurve end = null, float endTime = 1f
    ) {
        _onStart = new BetterEvent<Force>();
        _onChangeState = new BetterEvent<Force, ForceState>();
        _onEnd = new BetterEvent<Force>();
        _strength = strength;
        _direction = direction.normalized;
        _weight = Mathf.Min(weight, 1f);
        _mode = mode;
        if (start == null) {
            _start = AnimationCurve.Linear(0, 0, 1, 1);
        } else {
            _start = start;
        }
        _startDuration = startTime;
        if (end == null) {
            _end = AnimationCurve.Linear(0, 0, 1, 1);
        } else {
            _end = end;
        }
        _endDuration = endTime;
    }

    public Force(ForceData force, Vector2 direction, float weight, ForceMode mode)
        : this(force.Strength, direction, weight, mode, force.Acceleration, force.AccelerationTime, force.Deceleration, force.DecelerationTime) {

    }

    public Vector2 Evaluate() {
        return Evaluate(_currentPercent);
    }

    public Vector2 Evaluate(float percent) {
        Mathf.Clamp(0f, 1f, percent);
        switch (_state) {
            case ForceState.STAGNATION:
            case ForceState.ACCELERATION:
            default:
                return _direction * _start.Evaluate(percent) * _strength;
            case ForceState.DECELERATION:
                return _direction * _end.Evaluate(1f - percent) * _strength;
        }
    }

    public void Update(float deltaTime) {
        switch (_state) {
            case ForceState.ACCELERATION:
                if (_currentPercent == 0f) { _onStart.Invoke(this); }
                if (_startDuration != 0f) {
                    _currentPercent += deltaTime / _startDuration;
                } else {
                    ChangeState(ForceState.DECELERATION);
                    _currentPercent = 1f;
                    return;
                }

                if (_currentPercent >= 1f) {
                    switch (_mode) {
                        case ForceMode.TIMED:
                            ChangeState(ForceState.DECELERATION);
                            break;
                        case ForceMode.INPUT:
                        default:
                            _currentPercent = 1f;
                            ChangeState(ForceState.STAGNATION);
                            break;
                    }
                }
                break;
            case ForceState.DECELERATION:
                if (_currentPercent > 0f) {
                    _currentPercent -= deltaTime / _endDuration;
                    _currentPercent = Mathf.Max(0f, _currentPercent);
                    _onEnd.Invoke(this);
                }
                break;
            case ForceState.STAGNATION:
            default:
                break;
        }
    }

    public void Reset() {
        _currentPercent = 0f;
        ChangeState(ForceState.ACCELERATION);
    }

    public void ChangeState(ForceState state) {
        _state = state;
        _onChangeState.Invoke(this, state);
    }
}
