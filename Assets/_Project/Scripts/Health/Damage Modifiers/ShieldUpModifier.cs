using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUpModifier : DamageModifier {
    //[SerializeField] private EntityArmor entityArmor;
    private int damageToReduce;

    protected override bool Usable(int value, GameObject source) {
        //damageToReduce = entityArmor.ParryDamageReduction;
        //if (damageToReduce > 0) {
        //    //damageToReduce -= value;
        //    //if (damageToReduce < 0)
        //    //    damageToReduce = 0;

        //    //Value = damageToReduce;
        //    //entityArmor.CurrentArmor = damageToReduce;
        //    return true;
        //}
        //return false;
        return _value > 0;
    }
}
