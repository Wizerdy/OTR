using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPowerUp : MonoBehaviour {
    [SerializeField] DamageHealth _damageHealth;
    [SerializeField] PowerUp _powerUp;

    void Start() {
        _damageHealth.OnDamage += PowerUp;
    }

    void PowerUp(IHealth health, int damage) {
        try {
            GameObject root = health.GameObject.GetRoot();
            Debug.Log(root + " .. " + health.GameObject);
            if (root.CompareTag("Player")) {
                root.GetComponentInRoot<EntityAbilities>().Get<EntityPowerUp>().Add(_powerUp);
            }
        } catch (System.NullReferenceException e) {
            Debug.LogError("Error Handled : " + e);
        }
    }
}
