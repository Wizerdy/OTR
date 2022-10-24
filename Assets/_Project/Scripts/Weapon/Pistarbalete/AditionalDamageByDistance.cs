using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class AditionalDamageByDistance : MonoBehaviour {
    [SerializeField] AnimationCurve _damageByDistance;
    [SerializeField] int _damage = 5;
    [SerializeField] int _perfectDamage = 5;

    Vector2 _startPosition;

    public int PerfectDamage { get => _perfectDamage; set => _perfectDamage = value; }
    public Vector2 StartPosition => _startPosition;

    private void Start() {
        _startPosition = transform.Position2D();
    }

    public int ComputeDamages(float percentage) {
        return Mathf.CeilToInt(_damage * _damageByDistance.Evaluate(percentage)) + (percentage >= 1f ? _perfectDamage : 0);
    }
}
