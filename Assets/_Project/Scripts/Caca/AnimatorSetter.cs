using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorSetter : MonoBehaviour {
    [SerializeField] Animator _animator;
    [SerializeField] string _name;

    public void SetBool(bool value) {
        _animator.SetBool(_name, value);
    }

    public void SetInt(int value) {
        _animator.SetInteger(_name, value);
    }

    public void SetTrigger() {
        _animator.SetTrigger(_name);
    }

    public void ResetTrigger() {
        _animator.ResetTrigger(_name);
    }

    public void SetFloat(float value) {
        _animator.SetFloat(_name, value);
    }
}
