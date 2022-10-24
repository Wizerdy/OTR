using UnityEngine;

public class Force {
    public enum ForceMode {
        TIMED, INPUT
    }

    public enum ForceState {
        ACCELERATION, STAGNATION, DECELERATION
    }

    [SerializeField] float _strength;
    [SerializeField] Vector2 _direction;
    [SerializeField] float _weight;
    [SerializeField] AnimationCurve _start;
    [SerializeField] float _startDuration;
    [SerializeField] AnimationCurve _end;
    [SerializeField] float _endDuration;
    [SerializeField] ForceMode _mode;

    float _currentPercent = 0;
    ForceState _state;

    public float Strength { get { return _strength; } }
    public Vector2 Direction { get => _direction; }
    public float Weight { get { return _weight; } }
    public AnimationCurve Start { get { return _start; } }
    public AnimationCurve End { get { return _end; } }
    public ForceMode Mode { get { return _mode; } }

    public bool HasEnded { get => _state == ForceState.DECELERATION && _currentPercent >= 1; }

    public Force(float strength, Vector2 direction, float weight = 1f, ForceMode mode = ForceMode.TIMED, AnimationCurve start = null, AnimationCurve end = null) {
        _strength = strength;
        _direction = direction.normalized;
        _weight = weight;
        _mode = mode;
        if (start == null) {
            _start = AnimationCurve.Linear(0, 0, 1, 1);
        } else {
            _start = start;
        }
        if (end == null) {
            _end = AnimationCurve.Linear(0, 0, 1, 1);
        } else {
            _end = end;
        }
    }
    public Vector2 Evaluate() {
        return Evaluate(_currentPercent);
    }
    public Vector2 Evaluate(float percent) {
        Mathf.Clamp(0f, 1f, percent);
        switch (_state) {
            case ForceState.ACCELERATION:
                return _direction * _start.Evaluate(percent) * _strength;
            case ForceState.DECELERATION:
                return _direction * _end.Evaluate(percent) * _strength;
            case ForceState.STAGNATION:
            default:
                return _direction * _start.Evaluate(percent) * _strength;
        }
    }

    public void Update(float deltaTime) {
        switch (_state) {
            case ForceState.ACCELERATION:
                _currentPercent += deltaTime / _startDuration;
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
                _currentPercent += deltaTime / _endDuration;
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
