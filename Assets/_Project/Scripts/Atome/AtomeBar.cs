using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ToolsBoxEngine;
using UnityEngine.Events;

public class AtomeBar : MonoBehaviour {
    [SerializeField] Slider _slider;
    [SerializeField] float _maxValue;
    [SerializeField] float _minValue;
    [SerializeField] float _currentValue;
    [SerializeField, HideInInspector] BetterEvent<Slider> _onValueChanged = new BetterEvent<Slider>();

    public float Value { get => _currentValue; }
    public float MaxValue { get => _maxValue; }
    public float MinValue { get => _minValue; }

    public event UnityAction<Slider> OnValueChanged { add => _onValueChanged.AddListener(value); remove => _onValueChanged.RemoveListener(value); }

    private void Start() {
        UpdateSlider();
    }
    public void UpdateSlider() {
        if (_slider == null) return;
        _slider.value = Value;
        _slider.minValue = _minValue;
        _slider.maxValue = _maxValue;
        _onValueChanged?.Invoke(_slider);
    }

    public void Add(float value) {
        _currentValue += value;
        if (_currentValue > _maxValue) {
            _currentValue = _maxValue;
        }
        UpdateSlider();
    }

    public void Remove(float value, bool letOne) {
        _currentValue -= value;
        if (letOne) {
            if (_currentValue < _minValue + 1) {
                _currentValue = _minValue + 1;
            }
        } else {
            if (_currentValue < _minValue) {
                _currentValue = _minValue;
            }
        }
        UpdateSlider();
    }

    public void Remove(float value) {
        _currentValue -= value;
        if (_currentValue < _minValue) {
            _currentValue = _minValue;
        }
        UpdateSlider();
    }

    public float ChangeMaxValue(float newMaxValue) {
        _maxValue = newMaxValue;
        if (_maxValue < _currentValue) {
            _currentValue = _maxValue;
        }
        UpdateSlider();
        return _maxValue;
    }

    public float ChangeMinValue(float newMinValue) {
        _minValue = newMinValue;
        if (_minValue > _currentValue) {
            _currentValue = _minValue;
        }
        UpdateSlider();
        return _minValue;
    }
}
