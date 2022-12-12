using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PU_PowerUpByWeapon", menuName = "Scriptable Object/Power Up/PowerUp By Weapon", order = 4)]
public class PowerUpByWeapon : PowerUp {
    [System.Serializable]
    struct Weapon_PU {
        public WeaponType weapon;
        public PowerUp powerUp;
    }

    [SerializeField] List<Weapon_PU> _powerUps;

    PowerUp _powerUp;

    public override bool PlayParticles => _powerUp.PlayParticles;

    public override PowerUp Clone() {
        PowerUpByWeapon output = CreateInstance<PowerUpByWeapon>();
        output._powerUps = new List<Weapon_PU>(_powerUps);
        output._target = _target;
        output.SetEnable(false);
        output._cloned = true;
        return output;
    }

    protected override bool _Enable() {
        if (Enabled) { return false; }

        if (_target == null) { return false; }
        EntityWeaponry weaponry = _target.Get<EntityWeaponry>();
        if (weaponry == null) { return false; }

        if (_powerUp != null) {
            Disable();
        }

        _powerUp = Find(weaponry.Weapon);

        Debug.Log((weaponry.Weapon?.name ?? "Unhanded") + " .. " + _powerUp.name);
        if (_powerUp == null) { return false; }

        _powerUp = _powerUp.SetTarget(_target);
        _powerUp.Enable();
        _powerUp.OnDisable += _DisableMe;

        return _powerUp.Enabled;
    }

    protected override bool _Disable() {
        if (_powerUp == null) { return false; }

        _powerUp.Disable();
        _powerUp = null;

        return true;
    }

    private void _DisableMe(PowerUp _) {
        _Disable();
    }

    PowerUp Find(Weapon weapon) {
        WeaponType type = WeaponType.UNDEFINED;
        if (weapon != null) {
            type = weapon.Type;
        }
        for (int i = 0; i < _powerUps.Count; i++) {
            if (_powerUps[i].weapon == type) {
                return _powerUps[i].powerUp;
            }
        }
        return null;
    }
}
