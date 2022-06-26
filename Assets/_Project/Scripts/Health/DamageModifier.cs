using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageModifier : MonoBehaviour {
    public enum ResistanceType {
        RESISTANCE,
        WEAKNESS,
        FIX,
        IMMUNITY,
        NOMODIFIER
    }

    [SerializeField] ResistanceType type;
    [SerializeField] string damageType;
    [SerializeField] int value;

    [Space]
    [SerializeField] UnityEvent<DamageModifier, int> _onUse;

    public event UnityAction<DamageModifier, int> OnUse { add => _onUse.AddListener(value); remove => _onUse.RemoveListener(value); }

    public string DamageType { get { return damageType; } }
    public ResistanceType Resistance { get => type; set => type = value; }

    public int Modify(int amount) {
        switch (type) {
            case ResistanceType.WEAKNESS:
                amount += value;
                break;
            case ResistanceType.FIX:
                amount = value;
                break;
            case ResistanceType.RESISTANCE:
                amount = Mathf.Min(0, amount - value);
                break;
            case ResistanceType.IMMUNITY:
                amount = 0;
                break;
        }
        _onUse?.Invoke(this, amount);
        return amount;
    }
}

public static class DMMethod {
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
