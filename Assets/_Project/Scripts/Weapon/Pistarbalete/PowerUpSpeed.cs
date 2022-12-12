using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PU_Speed", menuName = "Scriptable Object/Power Up/Speed")]
public class PowerUpSpeed : PowerUp {
    [SerializeField] protected bool _playParticles = false;
    [SerializeField] float _speedFactor = 1f;

    EntityPhysicMovement _movements;

    public override bool PlayParticles => _playParticles;

    public override PowerUp Clone() {
        PowerUpSpeed output = CreateInstance<PowerUpSpeed>();
        output._speedFactor = _speedFactor;
        output._target = _target;
        output.SetEnable(false);
        output._cloned = true;
        output._playParticles = _playParticles;
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
