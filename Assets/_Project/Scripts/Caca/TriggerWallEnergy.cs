using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWallEnergy : MonoBehaviour {
    [SerializeField] SpriteRenderer _renderer;
    [SerializeField] float _speed = 1f;
    [SerializeField] AnimationCurve _alphaCurve = AnimationCurve.Constant(0f, 1f, 1f);

    Material _material;
    Coroutine _routine_wave;

    private void Reset() {
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        _material = _renderer.material;
    }

    public void TriggerWave(Vector2 position) {
        if (_routine_wave != null) { StopCoroutine(_routine_wave); }
        _material.SetVector("_Center", position);
        _material.SetFloat("_Step", 1f);
        _routine_wave = StartCoroutine(IWave());
    }

    public IEnumerator IWave() {
        float step = _material.GetFloat("_Step");
        while (step > 0f) {
            yield return null;
            _material.SetFloat("_Step", step - (Time.deltaTime * _speed));
            step = _material.GetFloat("_Step");
            Color color = _material.GetColor("_Color");
            color.a = _alphaCurve.Evaluate(step);
            _material.SetColor("_Color", color);
        }
        _material.SetFloat("_Step", 0f);
    }
}
