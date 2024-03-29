using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class Health : MonoBehaviour, IHealth {
    [SerializeField] int _maxHealth = 50;
    [SerializeField, ReadOnly] int _currentHealth;
    [SerializeField] bool _destroyOnDeath = true;
    [Space]
    [SerializeField] BetterEvent<int> _onHit = new BetterEvent<int>();
    [SerializeField] BetterEvent<int> _onHeal = new BetterEvent<int>();
    [SerializeField] BetterEvent _onDeath = new BetterEvent();
    [SerializeField] BetterEvent _onRevive = new BetterEvent();
    [SerializeField] List<DamageModifier> _damageModifiers = new List<DamageModifier>();

    [SerializeField, HideInInspector] BetterEvent<int> _onMaxHealthChange = new BetterEvent<int>();
    [SerializeField, HideInInspector] BetterEvent _onInvicible = new BetterEvent();
    [SerializeField, HideInInspector] BetterEvent _onVulnerable = new BetterEvent();

    Token _invicibilityToken = new Token();

    #region Properties

    public int MaxHealth { get => _maxHealth; set => SetMaxHealth(value); }
    public int CurrentHealth { get { return _currentHealth; } }
    public float Percentage { get { return MaxHealth > 0 ? ((float)_currentHealth / (float)MaxHealth) : 1f; } }
    public bool CanTakeDamage {
        get { return !_invicibilityToken.HasToken; }
        set { _invicibilityToken.AddToken(!value); }
    }
    public GameObject GameObject => gameObject;
    public bool IsDead => _currentHealth <= 0;

    public event UnityAction OnInvicible { add => _onInvicible.AddListener(value); remove => _onInvicible.RemoveListener(value); }
    public event UnityAction OnVulnerable { add => _onVulnerable.AddListener(value); remove => _onVulnerable.RemoveListener(value); }
    public event UnityAction<int> OnHit { add => _onHit.AddListener(value); remove => _onHit.RemoveListener(value); }
    public event UnityAction<int> OnHeal { add => _onHeal.AddListener(value); remove => _onHeal.RemoveListener(value); }
    public event UnityAction OnDeath { add => _onDeath.AddListener(value); remove => _onDeath.RemoveListener(value); }
    public event UnityAction OnRevive { add => _onRevive.AddListener(value); remove => _onRevive.RemoveListener(value); }
    public event UnityAction<int> OnMaxHealthChange { add => _onMaxHealthChange.AddListener(value); remove => _onMaxHealthChange.RemoveListener(value); }

    #endregion

    #region Unity Callbacks

    private void Awake() {
        _invicibilityToken.OnFill += () => _onInvicible?.Invoke();
        _invicibilityToken.OnEmpty += () => _onVulnerable?.Invoke();
        _currentHealth = _maxHealth;
    }

    #endregion

    public HealthData Save() {
        return new HealthData().Set(_maxHealth, _currentHealth);
    }

    public void Load(HealthData data) {
        if (data == null) { return; }
        _currentHealth = data.currentHealth;
        _maxHealth = data.maxHealth;
        _onMaxHealthChange.Invoke(_maxHealth);
    }

    public int TakeDamage(int amount, GameObject source = null) {
        if (IsDead) { return 0; }
        if (!CanTakeDamage) { return 0; }
        for (int i = 0; i < _damageModifiers.Count; i++) {
            if (!_damageModifiers[i].enabled) { continue; }
            amount = _damageModifiers[i].Use(amount, source);
        }

        if (amount <= 0) { return 0; }

        _currentHealth -= amount;
        _currentHealth = Mathf.Max(0, _currentHealth);
        _onHit?.Invoke(amount);

        if (_currentHealth <= 0) {
            Die();
        }

        return amount;
    }


    public int TakeDamage(int amount) {
        if (!CanTakeDamage) { return 0; }

        if (amount <= 0) { return 0; }

        _currentHealth -= amount;
        _currentHealth = Mathf.Max(0, _currentHealth);
        _onHit?.Invoke(amount);

        if (_currentHealth <= 0) {
            Die();
        }

        return amount;
    }

    public void TakeHeal(int amount) {
        if (amount <= 0 || _currentHealth >= _maxHealth) { return; }
        if (IsDead) { _onRevive.Invoke(); }
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth);
        _onHeal?.Invoke(amount);
    }

    public void TakeHeal(float amount) {
        if (amount <= 0 || _currentHealth >= _maxHealth) { return; }
        if (IsDead) { _onRevive.Invoke(); }
        int delta = Mathf.RoundToInt(_maxHealth * amount);
        if (delta + _currentHealth > _maxHealth) {
            delta = _maxHealth - _currentHealth;
        }
        _currentHealth += delta;
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth);
        _onHeal?.Invoke(delta);
    }

    public void Die() {
        _onDeath?.Invoke();
        if (_destroyOnDeath) {
            Destroy(gameObject);
        }
    }

    public void AddDamageModifier(DamageModifier dm) {
        if (dm == null) { return; }
        _damageModifiers.Add(dm);
    }

    public void RemoveDamageModifier(DamageModifier dm) {
        if (dm == null) { return; }
        if (_damageModifiers.Contains(dm)) {
            _damageModifiers.Remove(dm);
        }
    }

    public void SetMaxHealth(int amount) {
        if (amount == _maxHealth) { return; }
        int delta = amount - _maxHealth;
        if (_currentHealth == _maxHealth || _currentHealth > amount) { _currentHealth = amount; }
        _maxHealth = amount;
        _onMaxHealthChange?.Invoke(delta);
    }
}

public class HealthData {
    public int maxHealth;
    public int currentHealth;

    public HealthData Set(int maxHealth, int currentHealth) {
        this.maxHealth = maxHealth;
        this.currentHealth = currentHealth;
        return this;
    }
}
