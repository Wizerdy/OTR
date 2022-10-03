using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sloot;

public class EntityDash : MonoBehaviour {

    Vector2 _direction;
    float _speedReference;
    float _speedPercent;

    [SerializeField] EntityMovement _entityMovement;
    public Vector2 Direction { get => _direction; set => _direction = value; }
    public float SpeedReference { get => _speedReference; set => _speedReference = value; }
    public float SpeedPercent { get => _speedPercent; set => _speedPercent = value; }
    public float CurrentSpeed => _speedReference * _speedPercent;
    public Vector2 CurrentVelocity => CurrentSpeed * _direction;
    public IEnumerator Dash(float duration, AnimationCurve curve) {
        Timer timer = new Timer(this, duration).Start();
        while (timer.CurrentDuration < duration) {
            _entityMovement.Rigidbody.velocity = CurrentVelocity * curve.Evaluate(timer.CurrentDuration);
            yield return null;
        }
    }
}
