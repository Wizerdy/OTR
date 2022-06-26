using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class Health : MonoBehaviour, IHealth {
    [SerializeField] int _maxHealth = 50;
    [Space]
    [SerializeField] UnityEvent<int> _onHit;
    [SerializeField] UnityEvent<int> _onHeal;
    [SerializeField] UnityEvent _onDeath;
    [SerializeField] bool _destroyOnDeath = true;
    [SerializeField] List<DamageModifier> _damageModifiers = new List<DamageModifier>();

    [SerializeField, HideInInspector] UnityEvent<int> _onMaxHealthChange;
    [SerializeField, HideInInspector] UnityEvent _onInvicible;
    [SerializeField, HideInInspector] UnityEvent _onVulnerable;
    [SerializeField, HideInInspector] UnityEvent _onLateStart;

    Token _invicibilityToken = new Token();
    int _currentHealth;

    #region Properties

    public int MaxHealth { get => _maxHealth; set => SetMaxHealth(value); }
    public int CurrentHealth { get { return _currentHealth; } set { ChangeHealth(value - _currentHealth); } }
    public float Percentage { get { return MaxHealth == 0 ? _currentHealth / MaxHealth : 1f; } set { CurrentHealth = Mathf.RoundToInt(_maxHealth * value); } }
    public bool CanTakeDamage {
        get { return !_invicibilityToken.HasToken; }
        set { _invicibilityToken.AddToken(!value); }
    }

    public event UnityAction OnInvicible { add => _onInvicible.AddListener(value); remove => _onInvicible.RemoveListener(value); }
    public event UnityAction OnVulnerable { add => _onVulnerable.AddListener(value); remove => _onVulnerable.RemoveListener(value); }
    public event UnityAction<int> OnHit { add => _onHit.AddListener(value); remove => _onHit.RemoveListener(value); }
    public event UnityAction<int> OnHeal { add => _onHeal.AddListener(value); remove => _onHeal.RemoveListener(value); }
    public event UnityAction OnDeath { add => _onDeath.AddListener(value); remove => _onDeath.RemoveListener(value); }
    public event UnityAction<int> OnMaxHealthChange { add => _onMaxHealthChange.AddListener(value); remove => _onMaxHealthChange.RemoveListener(value); }
    public event UnityAction OnLateStart { add => _onLateStart.AddListener(value); remove => _onLateStart.RemoveListener(value); }

    #endregion

    #region Unity Callbacks

    private void Awake() {
        _invicibilityToken.OnFill += () => _onInvicible?.Invoke();
        _invicibilityToken.OnEmpty += () => _onVulnerable?.Invoke();
    }

    private void Start() {
        _currentHealth = _maxHealth;
        _onLateStart?.Invoke();
    }

    #endregion

    private void ChangeHealth(int amount) {
        if (amount == 0) { return; }
        if (amount < 0) {
            TakeDamage(amount);
        } else {
            TakeHeal(amount);
        }
    }

    public HealthData Save() {
        return new HealthData().Set(_maxHealth, _currentHealth);
    }

    public void Load(HealthData data) {
        if (data == null) { return; }
        _currentHealth = data.currentHealth;
        _maxHealth = data.maxHealth;
    } 

    public void TakeDamage(int amount, string damageType = "") {
        if (!CanTakeDamage) { return; }
            if (_damageModifiers.Contains(damageType)) {
                amount = _damageModifiers.Get(damageType).Modify(amount);
            }

            if (amount <= 0) { return; }

            _currentHealth -= amount;
            _currentHealth = Mathf.Max(0, _currentHealth);
            _onHit?.Invoke(amount);

            if (_currentHealth <= 0) {
                Die();
            }
    }

    public void TakeDamage(int amount) {
        _currentHealth -= amount;
        _currentHealth = Mathf.Max(0, _currentHealth);
        _onHit?.Invoke(amount);

        if (_currentHealth <= 0) {
            Die();
        }
    }

    public void TakeHeal(int amount) {
        if (amount <= 0 || _currentHealth >= _maxHealth) { return; }
        _currentHealth += amount;
        _currentHealth = Mathf.Min(_maxHealth, _currentHealth);
        _onHeal?.Invoke(amount);
    }

    public void Die() {
        _onDeath?.Invoke();
        if (_destroyOnDeath) {
            Destroy(gameObject);
        }
    }

    public void AddDamageModifier(DamageModifier dm) {
        _damageModifiers.Add(dm);
    }

    public void RemoveDamageModifier(DamageModifier dm) {
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

    //private void AddInvicibilityToken(int amount) {
    //    if (_invicibilityToken == 0 && amount > 0) {
    //        _onInvicible?.Invoke();
    //    } else if (_invicibilityToken > 0 && _invicibilityToken - amount <= 0) {
    //        _onVulnerable?.Invoke();
    //    }
    //    this.Hurl(_invicibilityToken.ToString());
    //    _invicibilityToken += amount;
    //    _invicibilityToken = Mathf.Max(0, _invicibilityToken);
    //}
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
