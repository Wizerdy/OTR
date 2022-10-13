using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Crossgun PowerUp", menuName = "Scriptable Object/Power Up/Crossgun")]
public class PowerUpCrossgun : PowerUp {
    [SerializeField] float _moveSpeedFactor;

    public override PowerUp Clone() {
        PowerUpCrossgun output = CreateInstance<PowerUpCrossgun>();
        output.SetTarget(_target);
        output._moveSpeedFactor = _moveSpeedFactor;
        return output;
    }

    public override void Disable() {
    }

    public override void Enable() {
        if (_target == null) { return; }

        Weapon weapon = _target.Get<EntityWeaponry>()?.Weapon;
        if (weapon == null) { return; }
        //weapon.MoveSpeed *= _moveSpeedFactor;
    }
}
