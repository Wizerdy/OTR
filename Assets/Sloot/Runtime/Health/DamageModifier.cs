using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sloot {
    public class DamageModifier : MonoBehaviour {
        public enum ModifierType {
            RESISTANCE,
            WEAKNESS,
            FIX,
            IMMUNITY,
            NOMODIFIER
        }

        [SerializeField] ModifierType _modifier;
        [SerializeField] string _modifierName;
        [SerializeField] int _value;

        [Space]
        [SerializeField] UnityEvent<DamageModifier, int> _onUse;

        public event UnityAction<DamageModifier, int> OnUse { add => _onUse.AddListener(value); remove => _onUse.RemoveListener(value); }

        public string DamageType { get { return _modifierName; } }
        public ModifierType Modifier { get => _modifier; set => _modifier = value; }

        public int Modify(int amount) {
            switch (_modifier) {
                case ModifierType.WEAKNESS:
                    amount += _value;
                    break;
                case ModifierType.FIX:
                    amount = _value;
                    break;
                case ModifierType.RESISTANCE:
                    amount = Mathf.Min(0, amount - _value);
                    break;
                case ModifierType.IMMUNITY:
                    amount = 0;
                    break;
            }
            _onUse?.Invoke(this, amount);
            return amount;
        }
    }

    public static class DamageModifierMethod {
        public static DamageModifier Get(this IEnumerable<DamageModifier> dms, string damageType) {
            foreach (var dm in dms) {
                if (dm.DamageType == damageType) {
                    return dm;
                }
            }
            return null;
        }
        public static bool Contains(this IEnumerable<DamageModifier> dms, string damageType) {
            foreach (var dm in dms) {
                if (dm.DamageType == damageType) {
                    return true;
                }
            }
            return false;
        }
    }
}