using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Force Data", menuName = "Scriptable Object/Force Data")]
public class ForceData : ScriptableObject {
    [Header("Movements")]
    [SerializeField] float _strength = 10f;
    [SerializeField] AnimationCurve _acceleration = AnimationCurve.Linear(0f, 0f, 1f, 1f);
    [SerializeField] float _accelerationTime = 1f;
    [SerializeField] AnimationCurve _deceleration = AnimationCurve.Linear(0f, 1f, 1f, 0f);
    [SerializeField] float _decelerationTime = 1f;

    public float Strength => _strength;
    public AnimationCurve Acceleration => _acceleration;
    public float AccelerationTime => _accelerationTime;
    public AnimationCurve Deceleration => _deceleration;
    public float DecelerationTime => _decelerationTime;
    public float Duration => _accelerationTime + _decelerationTime;
}
