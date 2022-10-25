using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PU_Damage", menuName = "Scriptable Object/Power Up/Damage")]
public class PowerUpDamage : PowerUp {
    [SerializeField] int _additionalDamage = 5;
    [SerializeField] bool _lostOnDrop = true;

    EntityWeaponry _targetWeaponry;

    public override PowerUp Clone() {
        PowerUpDamage output = CreateInstance<PowerUpDamage>();
        output._target = _target;
        output._additionalDamage = _additionalDamage;
        output._enabled = false;
        output._cloned = true;
        return output;
    }

    public override void Enable() {
        if (_enabled) { return; }
        if (_target == null) { return; }

        _targetWeaponry = _target.Get<EntityWeaponry>();
        if (_targetWeaponry == null) { return; }

        _targetWeaponry.DamageHealth?.DamageModifier.Add(_AdditionalDamage);
        _targetWeaponry.OnDrop += _Disable;
        _enabled = true;
    }

    public override void Disable() {
        if (!_enabled) { return; }
        if (_targetWeaponry == null) { return; }

        _targetWeaponry.DamageHealth?.DamageModifier.Remove(_AdditionalDamage);
        _targetWeaponry.OnDrop -= _Disable;

        _targetWeaponry = null;
        _enabled = false;
    }

    private int _AdditionalDamage(int damage) {
        return damage + _additionalDamage;
    }

    void _Disable(Weapon weapon) {
        if (!_lostOnDrop) { return; }
        Disable();
    }
}
