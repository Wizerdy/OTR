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
        output._enabled = false;
        output._cloned = true;
        return output;
    }

    public override void Enable() {
        if (_enabled) { return; }
        if (_target == null) { return; }

        _targetWeaponry = _target.Get<EntityWeaponry>();
        if (_targetWeaponry == null) { return; }
        if (_targetWeaponry.HasWeapon && _targetWeaponry.Weapon is BloodyFist fists) {
            _targetWeapon = fists;
            fists.BloodPointsOnHit += _additionalBloodPoint;
            _targetWeaponry.OnDrop += _Disable;
            _enabled = true;
        }
    }

    public override void Disable() {
        if (!_enabled) { return; }
        if (_targetWeapon != null) { _targetWeapon.BloodPointsOnHit -= _additionalBloodPoint; }
        if (_targetWeaponry != null) { _targetWeaponry.OnDrop -= _Disable; }

        _targetWeapon = null;
        _targetWeaponry = null;
        _enabled = false;
    }

    void _Disable(Weapon weapon) {
        if (!_lostOnDrop) { return; }
        Disable();
    }
}
