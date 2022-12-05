using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class BossHitFX : MonoBehaviour {
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] float _hitBlend = 0.4f;

    Coroutine _routine;

    void Start() {

    }

    void Update() {

    }

    public void HitMe(float time) {
        if (_routine != null) { StopCoroutine(_routine); }
        ChangeBlend(_hitBlend);
        StartCoroutine(Tools.Delay(() => ChangeBlend(0f), time));
    }

    public void ChangeBlend(float blend) {
        _renderer.material.SetFloat("_Blend", blend);
    }
}
