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

    public override PowerUp Clone() {
        PowerUpByWeapon output = CreateInstance<PowerUpByWeapon>();
        output._powerUps = new List<Weapon_PU>(_powerUps);
        output._target = _target;
        output._enabled = false;
        output._cloned = true;
        return output;
    }

    public override void Enable() {
        if (_enabled) { return; }

        if (_target == null) { return; }
        EntityWeaponry weaponry = _target.Get<EntityWeaponry>();
        if (weaponry == null) { return; }

        if (_powerUp != null) {
            Disable();
        }

        _powerUp = Find(weaponry.Weapon);

        if (_powerUp == null) { return; }

        _powerUp = _powerUp.SetTarget(_target);
        _powerUp.Enable();

        _enabled = _powerUp.Enabled;
    }

    public override void Disable() {
        if (_powerUp == null) { return; }

        _powerUp.Disable();
        _powerUp = null;
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
