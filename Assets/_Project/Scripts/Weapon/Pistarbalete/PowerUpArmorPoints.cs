using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PU_ArmorPoints", menuName = "Scriptable Object/Power Up/Armor Points")]
public class PowerUpArmorPoints : PowerUp {
    [SerializeField] protected bool _playParticles = false;
    [SerializeField] int _additionalArmorPoint = 1;
    [SerializeField, Range(0f, 2f)] float _regenArmorPointRateFactor = 2f;
    [SerializeField] bool _lostOnDrop = true;

    EntityWeaponry _targetWeaponry;
    EntityArmor _targetArmor;

    public override bool PlayParticles => _playParticles;

    public override PowerUp Clone() {
        PowerUpArmorPoints output = CreateInstance<PowerUpArmorPoints>();
        output._target = _target;
        output._additionalArmorPoint = _additionalArmorPoint;
        output.SetEnable(false);
        output._cloned = true;
        output._playParticles = _playParticles;
        return output;
    }

    protected override bool _Enable() {
        if (Enabled) { return false; }
        if (_target == null) { return false; }

        _targetWeaponry = _target.Get<EntityWeaponry>();
        if (_targetWeaponry == null) { return false; }
        if (_targetWeaponry.HasWeapon && _targetWeaponry.Weapon is AxeShield shield) {
            _targetArmor = _target.Get<EntityArmor>();
            _targetArmor.CurrentArmor += _additionalArmorPoint;
            _targetArmor.RegenRateArmor *= _regenArmorPointRateFactor;
            _targetWeaponry.OnDrop += _Disable;
            return true;
        }
        return false;
    }

    protected override bool _Disable() {
        if (!Enabled) { return false; }
        if (_targetWeaponry != null) { _targetWeaponry.OnDrop -= _Disable; }
        //shield.BloodPointsOnHit -= _additionalArmorPoint;
        //_targetWeapon.ArmorRegeneration /= 2;
        if (_regenArmorPointRateFactor != 0) {
            _targetArmor.RegenRateArmor /= _regenArmorPointRateFactor;
        }

        _targetWeaponry = null;
        _targetArmor = null;
        return true;
    }

    void _Disable(Weapon weapon) {
        if (!_lostOnDrop) { return; }
        Disable();
    }
}
