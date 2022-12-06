using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityPowerUp : MonoBehaviour, IEntityAbility {
    [SerializeField] EntityAbilities _abilities;
    [SerializeField] bool _stackable = false;

    List<PowerUp> _powerUps = new List<PowerUp>();

    public EntityAbilities Abilities => _abilities;

    public PowerUp Add(PowerUp powerUp) {
        PowerUp up = powerUp.SetTarget(_abilities);
        if (!_stackable) {
            _powerUps.Find((PowerUp pu) => { return pu.GetType() == up.GetType(); } )?.Disable();
        }
        _powerUps.Add(up);
        up.Enable();
        up.OnDisable += _Remove;
        return up;
    }

    public void Stock(PowerUp powerUp) {
        _powerUps.Add(powerUp);
    }

    void _Remove(PowerUp up) {
        _powerUps.Remove(up);
        up.SetTarget(null);
    }
}
