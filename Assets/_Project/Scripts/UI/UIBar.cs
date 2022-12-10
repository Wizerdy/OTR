using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class UIBar : MonoBehaviour {
    [SerializeField] Slider _slider;

    [Header("Value")]
    [SerializeField] float _currentValue = 0.5f;

    [Header("Fade Values")]
    [SerializeField] float _freezeTime = 0f;
    [SerializeField] float _fadeTime = 0.2f;
    [SerializeField] AnimationCurve _fadeCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);

    [SerializeField, HideInInspector] BetterEvent _onValueChanged = new BetterEvent();

    Coroutine _routine_FadeBar = null;

    public float Value { get => _currentValue; set => Set(value); }
    public event UnityAction OnValueChanged { add => _onValueChanged += value; remove => _onValueChanged -= value; }

    void Start() {
        _slider.maxValue = 1f;
        SplitSecondSet(_currentValue);
    }

    public void SplitSecondSet(float value) {
        _slider.value = value;
        if (_currentValue == value) { return; }
        _currentValue = Mathf.Min(value, 1f);
        _onValueChanged.Invoke();
    }

    public void Set(float value) {
        Set(value, _freezeTime, _fadeTime, _fadeCurve);
    }

    public void Set(float value, float freezeTime, float fadeTime, AnimationCurve fadeCurve) {
        if (_routine_FadeBar != null) { StopCoroutine(_routine_FadeBar); }
        _routine_FadeBar = StartCoroutine(FadeBar(value, freezeTime, fadeTime, fadeCurve));
        if (_currentValue == value) { return; }
        _currentValue = Mathf.Min(value, 1f);
        _onValueChanged.Invoke();
    }

    IEnumerator FadeBar(float targetValue, float freezeTime, float fadeTime, AnimationCurve fadeCurve) {
        yield return new WaitForSeconds(freezeTime);
        float currentValue = _slider.value;
        if (fadeTime <= 0f) { _slider.value = targetValue; yield break; }

        float timer = 0f;

        while (timer < fadeTime) {
            _slider.value = Mathf.Lerp(currentValue, targetValue, fadeCurve.Evaluate(timer/fadeTime));
            timer += Time.deltaTime;
            yield return null;
        }
        _slider.value = targetValue;
    }
}
