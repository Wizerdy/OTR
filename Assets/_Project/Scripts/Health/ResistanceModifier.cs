using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResistanceModifier : DamageModifier {
    [SerializeField] string _damageType;
    public string DamageType { get { return _damageType; } }

    protected override bool Usable(int amount, GameObject source) {
        DamageType damageType = source.GetComponentInRoot<DamageType>();
        if (damageType == null || !damageType.Type.Equals(_damageType)) { return false; }
        return true;
    }
}

//public static class DMMethod {
//    public static ResistanceModifier Get(this IEnumerable<ResistanceModifier> dms, string damageType) {
//        foreach (var dm in dms) {
//            if (dm.DamageType == damageType) {
//                return dm;
//            }
//        }
//        return null;
//    }

//    public static bool Contains(this IEnumerable<ResistanceModifier> dms, string damageType) {
//        foreach (var dm in dms) {
//            if (dm.DamageType == damageType) {
//                return true;
//            }
//        }
//        return false;
//    }
//}
