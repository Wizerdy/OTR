using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using static UnityEngine.ParticleSystem;

public class JouerSonParAttaque : MonoBehaviour {
    [SerializeField] int _firstAttack;
    [SerializeField] int _secondAttack;

    public void JouerSonByAttaqueType(AttackIndex type, Vector2 dir) {
        switch (type) {
            case AttackIndex.FIRST:
                SoundManager.Instance.PlaySfxByIndex(_firstAttack);
                break;
            case AttackIndex.SECOND:
                SoundManager.Instance.PlaySfxByIndex(_secondAttack);
                break;
        }
    }
}
