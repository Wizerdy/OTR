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
    [SerializeField] float _value;
    [SerializeField, HideInInspector] BetterEvent<Slider> _onValueChanged = new BetterEvent<Slider>();

    public float Value { get => _value; set { _value = value; UpdateSlider(); } }
    public float MaxValue { get => _maxValue; set { _maxValue = value; UpdateSlider(); } }
    public float MinValue { get => _minValue; set { _minValue = value; UpdateSlider(); } }

    public event UnityAction<Slider> OnValueChanged { add => _onValueChanged.AddListener(value); remove => _onValueChanged.RemoveListener(value); }
    void UpdateSlider() {
        if (_slider == null) return;
        _slider.value = Value;
        _slider.minValue = _minValue;
        _slider.maxValue = _maxValue;
        _onValueChanged?.Invoke(_slider);
    }

    public float Add(float value) {
        _value += value;
        if (_value > _maxValue) {
            _value = _maxValue;
        }
        return value;
    }

    public void Remove(float value) {
        _value -= value;
        if (_value < _minValue) {
            _value = _minValue;
        }
    }
}
