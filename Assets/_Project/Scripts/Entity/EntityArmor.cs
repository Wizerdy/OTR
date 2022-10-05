using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityArmor : DamageModifier, IEntityAbility {
    [SerializeField] private int armorCurrent;
    [SerializeField] private int armorMax;
    [SerializeField] private int armorRegenValue;
    [SerializeField] private float armorRegenRate;

    private float time;
    private bool isWieldingAxeShield = false;

    public int MaxArmor { get => armorMax; set => SetMaxArmor(value); }
    public int CurrentArmor { get => armorCurrent; set => SetCurrentArmor(value); }
    public int RegenValueArmor { get => armorRegenValue; set => SetRegenValue(value); }
    public float RegenRateArmor { get => armorRegenRate; set => SetRegenRate(value); }

    private void Update() {
        if (isWieldingAxeShield) {
            time -= Time.deltaTime;
            if (time <= 0) {
                armorCurrent += armorRegenValue;
                time = armorRegenRate;
            }
        }
    }

    public void ApplyArmor(int armorValue) {

    }

    public void OnPickUp() {
        isWieldingAxeShield = true;
    }

    public void ResetArmor() {
        armorCurrent = 0;
        isWieldingAxeShield = false;
    }

    public void SetMaxArmor(int value) {
        armorMax = value;
    }

    public void SetCurrentArmor(int value) {
        armorCurrent = value;
    }

    public void SetRegenValue(int value) {
        armorRegenValue = value;
    }

    public void SetRegenRate(float value) {
        armorRegenRate = value;
        time = armorRegenRate;
    }

    protected override bool Usable(int value, GameObject source) {
        Debug.Log("use armor");
        throw new System.NotImplementedException();
    }
}
