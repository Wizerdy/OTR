using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EntityCollisionArea))]
[RequireComponent(typeof(ColliderDelegate))]

public class AtomeMagnet : MonoBehaviour {
    [SerializeField] bool _isActive;
    public bool IsActive { get => _isActive; set => _isActive = value; }
    float _radius;
    EntityCollisionArea _entityCollisionArea;
    [SerializeField] string _componentToMagnet = "IHoldable";
    [SerializeField] AnimationCurve _force;
    [SerializeField] CircleCollider2D _circleCollider2D;
    Coroutine _coroutine = null;
    private void Reset() {
        _entityCollisionArea = GetComponent<EntityCollisionArea>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
    }

    private void Start() {
        if (_entityCollisionArea == null) {
            _entityCollisionArea = GetComponent<EntityCollisionArea>();
        }
        _radius = _circleCollider2D.radius;
    }

    void Update() {
        if (IsActive && !_entityCollisionArea.IsEmpty() && null == _coroutine) {
            GameObject nearest = null;
            try {
                nearest = _entityCollisionArea.Nearest(pair => pair.Key.gameObject.GetComponent(Type.GetType(_componentToMagnet)) != null);
            } catch {
                Debug.LogError("The Type \"" + _componentToMagnet + "\" is Not Recognise");
                IsActive = false;
            }
            //GameObject nearestIHoldable = _entityCollisionArea.Nearest();
            if (nearest != null) {
                _coroutine = StartCoroutine(Attact(nearest));
            }
        }
    }

    IEnumerator Attact(GameObject toMagnet) {
        if (!IsActive) {
            _coroutine = null;
            yield break;
        }
        Rigidbody2D rb = toMagnet.GetComponent<Rigidbody2D>();
        if (rb == null) {
            _coroutine = null;
            yield break;
        }
        while (IsActive) {
            Vector2 direction = transform.position - rb.transform.position;
            rb.velocity = direction.normalized * _force.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(0, _radius, direction.magnitude)));
            Debug.Log(rb.velocity);
            Debug.Log(_force.Evaluate(Mathf.Lerp(0, 1, Mathf.InverseLerp(0, _radius, direction.magnitude))));
            Debug.Log(Mathf.Lerp(0, 1, Mathf.InverseLerp(0, _radius, direction.magnitude)));
            Debug.Log(Mathf.InverseLerp(0, _radius, direction.magnitude));
            Debug.Log(direction.magnitude);
            yield return null;
        }
        _coroutine = null;
    }

}