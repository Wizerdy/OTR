using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine.BetterEvents;

public abstract class PowerUp : ScriptableObject {
    protected EntityAbilities _target;
    protected bool _cloned = false;
    protected BetterEvent<PowerUp> _onEnable = new BetterEvent<PowerUp>();
    protected BetterEvent<PowerUp> _onDisable = new BetterEvent<PowerUp>();

    private bool _enabled = false;

    public bool Enabled => _enabled;
    public abstract bool PlayParticles { get; }

    public event UnityAction<PowerUp> OnEnable { add => _onEnable += value; remove => _onEnable -= value; }
    public event UnityAction<PowerUp> OnDisable { add => _onDisable += value; remove => _onDisable -= value; }

    public PowerUp SetTarget(EntityAbilities ability) {
        PowerUp target = this;
        if (!_cloned) { target = Clone(); }
        target._target = ability;
        return target;
    }

    public void Enable() {
        if (!_Enable()) { return; }
        _enabled = true;
        _onEnable.Invoke(this);
    }

    public void Disable() {
        if (!_Disable()) { return; }
        _enabled = false;
        _onDisable.Invoke(this);
    }

    protected abstract bool _Enable();
    protected abstract bool _Disable();

    public abstract PowerUp Clone();

    protected void _DisableMe(PowerUp _) {
        Disable();
    }

    protected void SetEnable(bool state) {
        _enabled = state;
    }
}