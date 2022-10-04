using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using UnityEngine.Events;
using System;

public class EntityStorePoint : MonoBehaviour, IEntityAbility {
    [SerializeField] float _maxValue;
    [SerializeField] float _minValue = 0f;
    [SerializeField] float _currentValue;
    [SerializeField] BetterEvent _onValueChanged = new BetterEvent();
    [SerializeField] BetterEvent<float> _onMaxValueChanged = new BetterEvent<float>();
    [SerializeField] BetterEvent<float> _onMinValueChanged = new BetterEvent<float>();
    [SerializeField] BetterEvent<float> _onCurrentValueChanged = new BetterEvent<float>();
    [SerializeField] BetterEvent<float> _costStorePointDifference = new BetterEvent<float>();

    public event UnityAction OnValueChanged { add => _onValueChanged.AddListener(value); remove => _onValueChanged.RemoveListener(value); }
    public event UnityAction<float> CurrentValueChanged { add => _onCurrentValueChanged.AddListener(value); remove => _onCurrentValueChanged.RemoveListener(value); }
    public event UnityAction<float> MaxValueChanged { add => _onMaxValueChanged.AddListener(value); remove => _onMaxValueChanged.RemoveListener(value); }
    public event UnityAction<float> MinValueChanged { add => _onMinValueChanged.AddListener(value); remove => _onMinValueChanged.RemoveListener(value); }
    public event UnityAction<float> CostStorePointDifference { add => _costStorePointDifference.AddListener(value); remove => _costStorePointDifference.RemoveListener(value); }

    public float MinValue => _minValue;
    public float MaxValue => _maxValue;
    public float CurrentValue => _currentValue;
    public float GainStorePoint(float storePoint) {
        _currentValue += storePoint;
        if (_currentValue > _maxValue) {
            _currentValue = _maxValue;
        }
        _onValueChanged?.Invoke();
        _onCurrentValueChanged?.Invoke(_currentValue);
        return _currentValue;
    }
    public float LoseStorePoint(float storePoint) {
        _currentValue -= storePoint;
        if (_currentValue < _minValue) {
            _costStorePointDifference?.Invoke(Mathf.Abs(_currentValue - _minValue));
            _currentValue = _minValue;
        }
        _onValueChanged?.Invoke();
        _onCurrentValueChanged?.Invoke(_currentValue);
        return _currentValue;
    }

    public float ChangeMaxValue(float newMaxValue) {
        _maxValue = newMaxValue;
        if (_maxValue < _currentValue) {
            _currentValue = _maxValue;
            _onCurrentValueChanged?.Invoke(_currentValue);
        }
        _onValueChanged?.Invoke();
        _onMaxValueChanged?.Invoke(_maxValue);
        return _maxValue;
    }

    public float ChangeMinValue(float newMinValue) {
        _minValue = newMinValue;
        if (_minValue > _currentValue) {
            _currentValue = _minValue;
            _onCurrentValueChanged?.Invoke(_currentValue);
        }
        _onValueChanged?.Invoke();
        _onMinValueChanged?.Invoke(_minValue);
        return _minValue;
    }
}
