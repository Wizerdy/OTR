using UnityEngine;

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

    float _currentPercent = 0;
    ForceState _state = ForceState.ACCELERATION;

    public float Strength { get => _strength; set => _strength = value; }
    public Vector2 Direction { get => _direction; set => _direction = value; }
    public float Weight { get => _weight; set => _weight = value; }
    public AnimationCurve Start { get => _start; }
    public AnimationCurve End { get => _end; }
    public ForceMode Mode { get => _mode; }
    public ForceState State { get => _state; }

    public bool HasEnded { get => _state == ForceState.DECELERATION && _currentPercent <= 0; }

    public Force(float strength, Vector2 direction, float weight = 1f,
        ForceMode mode = ForceMode.TIMED,
        AnimationCurve start = null, float startTime = 1f,
        AnimationCurve end = null, float endTime = 1f) {
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
                if (_startDuration != 0f) {
                    _currentPercent += deltaTime / _startDuration;
                } else {
                    _state = ForceState.DECELERATION;
                    _currentPercent = 1f;
                    return;
                }

                if (_currentPercent >= 1f) {
                    switch (_mode) {
                        case ForceMode.TIMED:
                            _state = ForceState.DECELERATION;
                            break;
                        case ForceMode.INPUT:
                        default:
                            _currentPercent = 1f;
                            _state = ForceState.STAGNATION;
                            break;
                    }
                }
                break;
            case ForceState.DECELERATION:
                _currentPercent -= deltaTime / _endDuration;
                _currentPercent = Mathf.Max(0f, _currentPercent);
                break;
            case ForceState.STAGNATION:
            default:
                break;
        }
    }

    public void ChangeState(ForceState state) {
        _state = state;
    }
}
