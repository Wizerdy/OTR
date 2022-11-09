using Sloot;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldUp : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private ShieldUpModifier parryModifier;

    public void OnShieldUp() {
        health.AddDamageModifier(parryModifier);
    }

    public void OnShieldDown() {
        health.RemoveDamageModifier(parryModifier);
    }
}
