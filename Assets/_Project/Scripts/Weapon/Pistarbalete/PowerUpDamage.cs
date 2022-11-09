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
        output.SetEnable(false);
        output._cloned = true;
        return output;
    }

    protected override bool _Enable() {
        if (Enabled) { return false; }
        if (_target == null) { return false; }

        _targetWeaponry = _target.Get<EntityWeaponry>();
        if (_targetWeaponry == null) { return false; }

        _targetWeaponry.DamageHealth?.DamageModifier.Add(_AdditionalDamage);
        _targetWeaponry.OnDrop += _Disable;
        return true;
    }

    protected override bool _Disable() {
        if (!Enabled) { return false; }
        if (_targetWeaponry == null) { return false; }

        _targetWeaponry.DamageHealth?.DamageModifier.Remove(_AdditionalDamage);
        _targetWeaponry.OnDrop -= _Disable;

        _targetWeaponry = null;
        return true;
    }

    private int _AdditionalDamage(int damage) {
        return damage + _additionalDamage;
    }

    void _Disable(Weapon weapon) {
        if (!_lostOnDrop) { return; }
        Disable();
    }
}
