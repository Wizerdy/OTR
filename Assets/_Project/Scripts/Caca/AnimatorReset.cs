using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorReset : MonoBehaviour {
    [SerializeField] Animator _animator;

    public void ResetTriggers() {
        Animator animator = GetComponent<Animator>();
        for (int i = 0; i < animator.parameterCount; i++) {
            if (animator.GetParameter(i).type == AnimatorControllerParameterType.Trigger) {
                animator.ResetTrigger(i);
            }
        }
    }
}
