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
        output._enabled = false;
        output._cloned = true;
        return output;
    }

    public override void Enable() {
        if (_enabled) { return; }
        if (_target == null) { return; }

        _movements = _target.Get<EntityPhysicMovement>();
        if (_movements == null) { return; }

        _movements.AddSpeedModifier(_speedFactor);
        _enabled = true;
    }

    public override void Disable() {
        if (!_enabled) { return; }
        if (_movements == null) { return; }

        _movements.RemoveSpeedModifier(_speedFactor);

        _enabled = false;
    }
}
