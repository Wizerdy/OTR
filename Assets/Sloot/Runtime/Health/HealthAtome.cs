using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sloot {
    public class HealthAtome : MonoBehaviour {
        [SerializeField] int _maxHealth = 50;
        [SerializeField] int _currentHealth;
        [Space]
        [SerializeField] UnityEvent<int> _onHit;
        [SerializeField] UnityEvent<int> _onHeal;
        [SerializeField] List<DamageModifier> _damageModifiers = new List<DamageModifier>();

        [SerializeField, HideInInspector] UnityEvent _onInvicible;
        [SerializeField, HideInInspector] UnityEvent _onVulnerable;


        #region Properties

        public event UnityAction<int> OnHit { add => _onHit.AddListener(value); remove => _onHit.RemoveListener(value); }
        public event UnityAction<int> OnHeal { add => _onHeal.AddListener(value); remove => _onHeal.RemoveListener(value); }

        #endregion

        #region Unity Callbacks

        private void Start() {
            _currentHealth = _maxHealth;
        }

        #endregion

        public void TakeDamage(int amount, string modifierName = "") {
            if (_damageModifiers.Contains(modifierName)) {
                amount = _damageModifiers.Get(modifierName).Modify(amount);
            }

            if (amount <= 0) { return; }

            _currentHealth -= amount;
            _currentHealth = Mathf.Max(0, _currentHealth);
            _onHit?.Invoke(amount);
        }

        public void TakeHeal(int amount) {
            if(amount <= 0) { return; }
            _currentHealth += amount;
            _currentHealth = Mathf.Min(_maxHealth, _currentHealth);
            _onHeal?.Invoke(amount);
        }

        public void AddDamageModifier(DamageModifier dm) {
            _damageModifiers.Add(dm);
        }

        public void RemoveDamageModifier(DamageModifier dm) {
            if (_damageModifiers.Contains(dm)) {
                _damageModifiers.Remove(dm);
            }
        }
    }
}
