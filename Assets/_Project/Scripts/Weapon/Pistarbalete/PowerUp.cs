using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : ScriptableObject {
    protected EntityAbilities _target;

    public void SetTarget(EntityAbilities ability) {
        _target = ability;
    }

    public abstract void Enable();
    public abstract void Disable();

    public abstract PowerUp Clone();
}