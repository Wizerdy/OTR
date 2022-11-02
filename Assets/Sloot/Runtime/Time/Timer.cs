using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer {
    float _duration = 1f;
    float _currentDuration = 0f;
    bool _pause = false;
    bool _loop = true;
    bool _isWorking = false;
    Coroutine coroutine;
    MonoBehaviour _instigator;
    public float Duration { get => _duration; set => _duration = value; }
    public float CurrentDuration => _currentDuration;

    public bool IsWorking => _isWorking;

    [SerializeField] UnityEvent _onActivate;

    public event UnityAction OnActivate { add => _onActivate.AddListener(value); remove => _onActivate.RemoveListener(value); }

    public Timer(MonoBehaviour instigator, float duration, bool loop = true) {
        _instigator = instigator;
        _onActivate = new UnityEvent();
        _duration = duration;
        _loop = loop;
    }

    public Timer Start(float duration, float offset = 0) {
        _currentDuration = duration;
        Start(offset);
        return this;
    }
    public Timer Start(float offset = 0) {
        _pause = false;
        End();
        _isWorking = true;
        coroutine = _instigator.StartCoroutine(StartTimer(offset));
        return this;
    }

    public void Pause() {
        _pause = true;
        _isWorking = false;
    }

    public void Continue() {
        _pause = false;
        _isWorking = true;
    }

    public void End() {
        if (coroutine != null)
            _instigator.StopCoroutine(coroutine);
        _isWorking = false;
    }

    IEnumerator StartTimer(float offset) {
        if (0 < offset)
            yield return new WaitForSeconds(offset);
        do {
            _currentDuration = 0f;
            while (_currentDuration < _duration) {
                yield return null;
                if (!_pause)
                    _currentDuration += Time.deltaTime;
            }
            _onActivate.Invoke();
        } while (true && _loop);
        End();
    }
}
