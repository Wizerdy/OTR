using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PU_Speed", menuName = "Scriptable Object/Power Up/Speed")]
public class PowerUpSpeed : PowerUp {
    [SerializeField] float _speedFactor = 1f;

    EntityPhysicMovement _movements;

    public override PowerUp Clone() {
        PowerUpSpeed output = CreateInstance<PowerUpSpeed>();
        output._speedFactor = _speedFactor;
        output._target = _target;
        output.SetEnable(false);
        output._cloned = true;
        return output;
    }

    protected override bool _Enable() {
        if (Enabled) { return false; }
        if (_target == null) { return false; }

        _movements = _target.Get<EntityPhysicMovement>();
        if (_movements == null) { return false; }

        _movements.AddSpeedModifier(_speedFactor);
        return true;
    }

    protected override bool _Disable() {
        if (!Enabled) { return false; }
        if (_movements == null) { return false; }

        _movements.RemoveSpeedModifier(_speedFactor);

        return true;
    }
}
