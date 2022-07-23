using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResistanceModifier : DamageModifier {
    [SerializeField] DamageType _damageType;

    public DamageType DamageType { get => _damageType.Clone(); } // Passage par copie

    protected override bool Usable(int amount, GameObject source) {
        DamageType damageType = source.GetComponentInRoot<DamageType>();
        if (damageType == null || !damageType.Type.Equals(_damageType)) { return false; }
        return true;
    }
}
