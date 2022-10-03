using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct StatusEffect {
    public ScriptableStatusEffect statusEffect;
    public float currentEffectDuration;

    public StatusEffect(ScriptableStatusEffect SE, float duration) {
        statusEffect = SE;
        currentEffectDuration = duration;
    }
}
public class EntityStatusEffect : MonoBehaviour, IBuffable {
    private List<StatusEffect> statusEffects = new List<StatusEffect>();
    private int index;

    private void Update() {
        for (int i = 0; i < statusEffects.Count; i++) {
            if (statusEffects[i].statusEffect != null) {
                DoEffect(Time.deltaTime, i);
            }
        }
    }

    public void ApplyBuff(ScriptableStatusEffect effect) {
        var se = new StatusEffect(effect, effect.duration);
    }

    public void RemoveBuff(ScriptableStatusEffect effect) {
        for (int i = 0; i < statusEffects.Count; i++) {
            if (statusEffects[i].statusEffect == effect) {
                index = statusEffects.IndexOf(statusEffects[i]);
            }
        }
        statusEffects.Remove(statusEffects[index]);
    }

    public void DoEffect(float time, int index) {

        var effect = statusEffects[index];
        effect.currentEffectDuration -= time;

        if (effect.currentEffectDuration <= 0) {
            RemoveBuff(statusEffects[index].statusEffect);
        } else {
            statusEffects[index] = effect;
        }
    }
}
