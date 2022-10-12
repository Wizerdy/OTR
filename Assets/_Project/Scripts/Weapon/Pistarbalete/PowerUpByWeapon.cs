using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PowerUpByWeapon", menuName = "Scriptable Object/Power Up/PowerUpByWeapon", order = 4)]
public class PowerUpByWeapon : PowerUp {
    [SerializeField] List<(Weapon, PowerUp)> _powerUps;

    PowerUp _powerUp;

    public override void Enable() {
        if (_target == null) { return; }
        EntityWeaponry weaponry = _target.Get<EntityWeaponry>();
        if (weaponry == null || !weaponry.HasWeapon) { return; }

        if (_powerUp != null) {
            Disable();
        }

        _powerUp = Find(weaponry.Weapon);

        if (_powerUp == null) { return; }

        _powerUp = _powerUp.Clone();
        _powerUp.SetTarget(_target);
        _powerUp.Enable();
    }

    public override void Disable() {
        _powerUp.Disable();
        _powerUp = null;
    }

    public override PowerUp Clone() {
        PowerUpByWeapon output = CreateInstance<PowerUpByWeapon>();
        output.SetTarget(_target);
        return output;
    }

    PowerUp Find(Weapon weapon) {
        for (int i = 0; i < _powerUps.Count; i++) {
            if (_powerUps[i].Item1 == weapon) {
                return _powerUps[i].Item2;
            }
        }
        return null;
    }
}
