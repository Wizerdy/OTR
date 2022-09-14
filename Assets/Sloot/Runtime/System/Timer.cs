using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer {
    float _duration = 1f;
    float _currentDuration = 0f;
    Coroutine coroutine;
    MonoBehaviour _instigator;
    public float Duration { get => _duration; set => _duration = value; }
    public float CurrentDuration => _currentDuration;

    [SerializeField] UnityEvent _onActivate;

    public event UnityAction OnActivate { add => _onActivate.AddListener(value); remove => _onActivate.RemoveListener(value); }

    public Timer(MonoBehaviour instigator, float duration) {
        _instigator = instigator;
        _onActivate = new UnityEvent();
        _duration = duration;
    }

    public void Start(float duration, float offset = 0) {
        _currentDuration = duration;
        End();
        coroutine = _instigator.StartCoroutine(StartTimer(offset));
    }
    public void Start(float offset = 0) {
        End();
        coroutine = _instigator.StartCoroutine(StartTimer(offset));
    }

    public void End() {
        if (coroutine != null)
            _instigator.StopCoroutine(coroutine);
    }

    IEnumerator StartTimer(float offset) {
        if (0 < offset)
            yield return new WaitForSeconds(offset);
        while (true) {
            _currentDuration = 0f;
            while (_currentDuration < _duration) {
                yield return null;
                _currentDuration += Time.deltaTime;
            }
            _onActivate.Invoke();
        }
    }


}
