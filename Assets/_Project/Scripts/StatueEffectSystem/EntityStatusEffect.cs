using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityStatusEffect : MonoBehaviour, IBuffable {
    private List<ScriptableStatusEffect> statusEffects = new List<ScriptableStatusEffect>();
    private List<float> currentEffectDuration = new List<float>();

    private void Update() {
        for (int i = 0; i < statusEffects.Count; i++) {
            if (statusEffects[i] != null) {
                DoEffect(Time.deltaTime, i);
            }
        }
    }

    public void ApplyBuff(ScriptableStatusEffect effect) {
        statusEffects.Add(effect);
        currentEffectDuration.Add(effect.duration);
    }

    public void RemoveBuff(ScriptableStatusEffect effect) {
        var index = statusEffects.IndexOf(effect);
        statusEffects.Remove(effect);
        currentEffectDuration.Remove(index);
    }

    public void DoEffect(float time, int index) {

        currentEffectDuration[index] += Time.deltaTime;

        if (currentEffectDuration[index] >= statusEffects[index].duration) {
            RemoveBuff(statusEffects[index]);
        }
    }
}
