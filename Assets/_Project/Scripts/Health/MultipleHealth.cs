using System.Collections;
using System.Collections.Generic;
using ToolsBoxEngine.BetterEvents;
using UnityEngine;
using UnityEngine.Events;

public class MultipleHealth : MonoBehaviour {
    Health _health;
    [SerializeField] List<int> _healths = new List<int>();
    [SerializeField] int _currentHealth = -1;

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
        Destroy(gameObject.transform.parent.gameObject);
    }
}
