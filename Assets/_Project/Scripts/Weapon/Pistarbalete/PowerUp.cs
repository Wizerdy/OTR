using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : ScriptableObject {
    protected EntityAbilities _target;
    protected bool _enabled = false;
    protected bool _cloned = false;

    public bool Enabled => _enabled;

    public PowerUp SetTarget(EntityAbilities ability) {
        PowerUp target = this;
        if (!_cloned) { target = Clone(); }
        target._target = ability;
        return target;
    }

    public abstract void Enable();
    public abstract void Disable();

    public abstract PowerUp Clone();
}