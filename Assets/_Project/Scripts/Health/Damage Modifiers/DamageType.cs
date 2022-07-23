using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DamageType : MonoBehaviour {
    [SerializeField] string _damageType;

    public string Type => _damageType;

    public DamageType(DamageType damageType) {
        _damageType = damageType.Type;
    }

    public DamageType Clone() {
        return new DamageType(this);
    }
}
