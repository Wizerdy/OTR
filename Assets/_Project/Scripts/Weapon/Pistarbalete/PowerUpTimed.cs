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
        output._enabled = false;
        output._cloned = true;
        return output;
    }

    public override void Enable() {
        if (_enabled) { return; }
        if (_target == null) { return; }

        _powerUpInstancied = _powerUp.SetTarget(_target);
        _powerUpInstancied.Enable();

        _routine_timer = CoroutinesManager.Start(
            Tools.Delay(
                (PowerUpTimed pu) => { _routine_timer = null; pu.Disable(); },
                this,
                _time));

        _enabled = _powerUpInstancied.Enabled;
    }

    public override void Disable() {
        if (!_enabled) { return; }
        if (_routine_timer != null) { CoroutinesManager.Stop(_routine_timer); }
        _powerUpInstancied?.Disable();
        _enabled = false;
    }

    private void OnDestroy() {
        if (_routine_timer != null) { CoroutinesManager.Stop(_routine_timer); }
    }
}
