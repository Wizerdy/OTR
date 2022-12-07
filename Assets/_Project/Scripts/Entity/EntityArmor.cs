using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityArmor : MonoBehaviour, IEntityAbility {
    [SerializeField] private Health health;
    [SerializeField] private ArmorModifier armorModifier;
    private int armorCurrent;
    private int armorMax;
    private int armorRegenValue;
    private float armorRegenRate;
    private int parryDamageReduction;

    private float time;
    private bool isWieldingAxeShield = false;
    bool blocking = false;

    public int MaxArmor { get => armorMax; set => SetMaxArmor(value); }
    public int CurrentArmor { get => armorCurrent; set => SetCurrentArmor(value); }
    public int RegenValueArmor { get => armorRegenValue; set => SetRegenValue(value); }
    public float RegenRateArmor { get => armorRegenRate; set => SetRegenRate(value); }
    public int ParryDamageReduction { get => parryDamageReduction; set => SetParryDamageReduction(value); }
    public bool Blockin { get => blocking; set => blocking = value; }

    private void Update() {
        if (isWieldingAxeShield) {
            time -= Time.deltaTime;
            if (time <= 0) {
                if (armorCurrent < armorMax)
                    armorCurrent += armorRegenValue;
                time = armorRegenRate;
            }
        }
    }

    public void OnPickUp() {
        isWieldingAxeShield = true;
        health.AddDamageModifier(armorModifier);
    }

    public void ResetArmor() {
        armorCurrent = 0;
        health.RemoveDamageModifier(armorModifier);
        isWieldingAxeShield = false;
    }

    private void SetMaxArmor(int value) {
        armorMax = value;
    }

    private void SetCurrentArmor(int value) {
        armorCurrent = value;
    }

    private void SetRegenValue(int value) {
        armorRegenValue = value;
    }

    private void SetRegenRate(float value) {
        armorRegenRate = value;
        time = armorRegenRate;
    }
    private void SetParryDamageReduction(int value) {
        parryDamageReduction = value;
    }


}
