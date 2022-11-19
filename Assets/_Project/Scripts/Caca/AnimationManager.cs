using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour {
    [SerializeField] Animator _animator;

    private void Start() {
        _animator = GetComponent<Animator>();
    }
    public void SetAnimationBoolTrue(string animation) {
        _animator.SetBool(animation, true);
    }

    public void SetAnimationBoolFalse(string animation) {
        _animator.SetBool(animation, false);
    }
}
