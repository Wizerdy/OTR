using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PU_BloodPoints", menuName = "Scriptable Object/Power Up/Blood Points")]
public class PowerUpBloodPoint : PowerUp
{
    [SerializeField] int _additionalBloodPoint = 5;
    [SerializeField] bool _lostOnDrop = true;

    EntityWeaponry _targetWeaponry;
    BloodyFist _targetWeapon;

    public override PowerUp Clone() {
        PowerUpBloodPoint output = CreateInstance<PowerUpBloodPoint>();
        output._target = _target;
        output._additionalBloodPoint = _additionalBloodPoint;
        output.SetEnable(false);
        output._cloned = true;
        return output;
    }

    protected override bool _Enable() {
        if (Enabled) { return false; }
        if (_target == null) { return false; }

        _targetWeaponry = _target.Get<EntityWeaponry>();
        if (_targetWeaponry == null) { return false; }
        if (_targetWeaponry.HasWeapon && _targetWeaponry.Weapon is BloodyFist fists) {
            _targetWeapon = fists;
            //fists.BloodPointsOnHit += _additionalBloodPoint;
            _targetWeaponry.OnDrop += _Disable;
            return true;
        }
        return false;
    }

    protected override bool _Disable() {
        if (!Enabled) { return false; }
        if (_targetWeapon != null) { _targetWeapon.BloodPointsOnHit -= _additionalBloodPoint; }
        if (_targetWeaponry != null) { _targetWeaponry.OnDrop -= _Disable; }

        _targetWeapon = null;
        _targetWeaponry = null;
        return true;
    }

    void _Disable(Weapon weapon) {
        if (!_lostOnDrop) { return; }
        Disable();
    }
}
