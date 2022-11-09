using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PU_ArmorPoints", menuName = "Scriptable Object/Power Up/Armor Points")]
public class PowerUpArmorPoints : PowerUp
{
    [SerializeField] int _additionalArmorPoint = 5;
    [SerializeField] bool _lostOnDrop = true;

    EntityWeaponry _targetWeaponry;
    AxeShield _targetWeapon;

    public override PowerUp Clone() {
        PowerUpArmorPoints output = CreateInstance<PowerUpArmorPoints>();
        output._target = _target;
        output._additionalArmorPoint = _additionalArmorPoint;
        output.SetEnable(false);
        output._cloned = true;
        return output;
    }

    protected override bool _Enable() {
        if (Enabled) { return false; }
        if (_target == null) { return false; }

        _targetWeaponry = _target.Get<EntityWeaponry>();
        if (_targetWeaponry == null) { return false; }
        if (_targetWeaponry.HasWeapon && _targetWeaponry.Weapon is AxeShield shield) {
            _targetWeapon = shield;
            //shield.BloodPointsOnHit += _additionalArmorPoint;
            _targetWeaponry.OnDrop += _Disable;
            return true;
        }
        return false;
    }

    protected override bool _Disable() {
        if (!Enabled) { return false; }
        if (_targetWeaponry != null) { _targetWeaponry.OnDrop -= _Disable; }
        //shield.BloodPointsOnHit -= _additionalArmorPoint;

        _targetWeapon = null;
        _targetWeaponry = null;
        return true;
    }

    void _Disable(Weapon weapon) {
        if (!_lostOnDrop) { return; }
        Disable();
    }
}
