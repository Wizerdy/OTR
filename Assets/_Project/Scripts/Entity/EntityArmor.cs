using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EntityArmor : MonoBehaviour, IEntityAbility {
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
                if (armorCurrent < armorMax)
                    armorCurrent += armorRegenValue;
                time = armorRegenRate;
            }
        }
    }

    private void ApplyArmor(int armorValue) {

    }

    public void OnPickUp() {
        isWieldingAxeShield = true;
    }

    public void ResetArmor() {
        armorCurrent = 0;
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
}
