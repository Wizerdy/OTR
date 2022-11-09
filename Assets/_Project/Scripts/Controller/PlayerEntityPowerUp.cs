using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityPowerUp : MonoBehaviour {
    [SerializeField] EntityPowerUp _entityPowerUp;
    [Header("Power Ups")]
    [SerializeField] PowerUp _pickupPowerUp;

    void Start() {
        _entityPowerUp.Abilities.Get<EntityWeaponry>().OnPickup += _Pickup;
    }

    private void _Pickup(Weapon weapon) {
        _entityPowerUp.Add(_pickupPowerUp);
    }
}
