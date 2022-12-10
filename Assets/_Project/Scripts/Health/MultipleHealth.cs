using System.Collections;
using System.Collections.Generic;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;
using UnityEngine;
using UnityEngine.Events;

public class MultipleHealth : MonoBehaviour {
    [SerializeField] List<int> _healths = new List<int>();
    [SerializeField, ReadOnly] int _currentHealth = -1;
    [SerializeField] BetterEvent _onRealDeath = new BetterEvent();

    Health _health;

    public int CurrentIndex => _currentHealth;
    public event UnityAction OnRealDeath { add => _onRealDeath.AddListener(value); remove => _onRealDeath.RemoveListener(value); }

    private void Start() {
        _health = GetComponent<Health>();
        _currentHealth = -1;
        NewHealth();
    }

    public void NewHealth() {
        _currentHealth++;
        if(_currentHealth >= _healths.Count) {
            Die();
            return;
        } 
        HealthData healthData = new HealthData().Set(_healths[_currentHealth], _healths[_currentHealth]);
        _health.Load(healthData);
    }

    void Die() {
        _onRealDeath?.Invoke();
    }
}
