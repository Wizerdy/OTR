using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class DH_DestroyAfterTime : MonoBehaviour {
    [SerializeField] float _time = 1f;

    DamageHealth _damageHealth;

    void Start() {
        _damageHealth = GetComponent<DamageHealth>();
        _damageHealth.OnDead += _Countdown;
    }

    void _Countdown() {
        StartCoroutine(Tools.Delay((DamageHealth dh) => dh.Die(), _damageHealth, _time));
    }
}
