using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorModifier : DamageModifier {
    [SerializeField] private EntityArmor entityArmor;
    private int currentArmor;

    // take entity armor put in here _value
    protected override bool Usable(int value, GameObject source) {
        currentArmor = entityArmor.CurrentArmor;
        if (currentArmor > 0) {
            currentArmor -= value;
            if (currentArmor < 0)
                currentArmor = 0;

            Value = currentArmor;
            entityArmor.CurrentArmor = currentArmor;
            return true;
        }
        return false;
    }
}
