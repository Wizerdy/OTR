using System.Collections;
using System.Collections.Generic;
using ToolsBoxEngine;
using UnityEngine;

[CreateAssetMenu(fileName = "PU_Timed", menuName = "Scriptable Object/Power Up/Timed", order = 4)]
public class PowerUpTimed : PowerUp {
    [SerializeField] PowerUp _powerUp;
    [SerializeField] float _time = 5f;

    Coroutine _routine_timer;
    PowerUp _powerUpInstancied;

    public override PowerUp Clone() {
        PowerUpTimed output = CreateInstance<PowerUpTimed>();
        output._target = _target;
        output._powerUp = _powerUp;
        output._time = _time;
        output.SetEnable(false);
        output._cloned = true;
        return output;
    }

    protected override bool _Enable() {
        if (Enabled) { Renable(); }
        if (_target == null) { return false; }

        _powerUpInstancied = _powerUp.SetTarget(_target);
        _powerUpInstancied.Enable();

        _routine_timer = CoroutinesManager.Start(
            Tools.Delay(
                (PowerUpTimed pu) => { _routine_timer = null; pu.Disable(); },
                this,
                _time));

        return _powerUpInstancied.Enabled;
    }

    private void Renable() {
        if (_routine_timer != null) { CoroutinesManager.Stop(_routine_timer); }
    }

    protected override bool _Disable() {
        if (!Enabled) { return false; }
        if (_routine_timer != null) { CoroutinesManager.Stop(_routine_timer); }
        _powerUpInstancied?.Disable();

        return true;
    }

    private void OnDestroy() {
        if (_routine_timer != null) { CoroutinesManager.Stop(_routine_timer); }
    }
}
