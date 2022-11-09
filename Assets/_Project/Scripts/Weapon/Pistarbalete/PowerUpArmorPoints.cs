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
        output._enabled = false;
        output._cloned = true;
        return output;
    }

    public override void Enable() {
        if (_enabled) { return; }
        if (_target == null) { return; }

        _targetWeaponry = _target.Get<EntityWeaponry>();
        if (_targetWeaponry == null) { return; }
        if (_targetWeaponry.HasWeapon && _targetWeaponry.Weapon is AxeShield shield) {
            _targetWeapon = shield;
            //shield.BloodPointsOnHit += _additionalArmorPoint;
            _targetWeaponry.OnDrop += _Disable;
            _enabled = true;
        }
    }

    public override void Disable() {
        if (!_enabled) { return; }
        if (_targetWeaponry != null) { _targetWeaponry.OnDrop -= _Disable; }
        //shield.BloodPointsOnHit -= _additionalArmorPoint;

        _targetWeapon = null;
        _targetWeaponry = null;
        _enabled = false;
    }

    void _Disable(Weapon weapon) {
        if (!_lostOnDrop) { return; }
        Disable();
    }
}
