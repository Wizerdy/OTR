using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class EntityFieldOfView : MonoBehaviour, IEntityAbility {
    [SerializeField] bool _blind = false;
    [SerializeField] Vector2 _direction = Vector2.up;
    [SerializeField, Range(1f, 360f)] float _angle = 65f;
    [SerializeField] int _raysCount = 5;
    [SerializeField] float _raysDistance = 5f;
    [SerializeField] float _updateDeltaTime = 0.1f;
    [SerializeField] bool _triggersWatch = false;

    //[SerializeField] BetterEvent<RaycastHit2D> _onStartView = new BetterEvent<RaycastHit2D>();
    [SerializeField] BetterEvent<RaycastHit2D> _onView = new BetterEvent<RaycastHit2D>();
    //[SerializeField] BetterEvent<RaycastHit2D> _onEndView = new BetterEvent<RaycastHit2D>();

    public Vector2 Direction { get => _direction; set => _direction = value; }

    //public event UnityAction<RaycastHit2D> OnStartView { add => _onStartView.AddListener(value); remove => _onStartView.RemoveListener(value); }
    public event UnityAction<RaycastHit2D> OnView { add => _onView.AddListener(value); remove => _onView.RemoveListener(value); }
    //public event UnityAction<RaycastHit2D> OnEndView { add => _onEndView.AddListener(value); remove => _onEndView.RemoveListener(value); }

    private void OnEnable() {
        StartCoroutine(UpdateWatch(_updateDeltaTime));
    }

    IEnumerator UpdateWatch(float deltaTime) {
        if (_raysCount <= 0) { yield break; }
        WaitForSeconds timer = new WaitForSeconds(deltaTime);
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = _triggersWatch;
        List<RaycastHit2D> lastTargets = new List<RaycastHit2D>();
        List<Rigidbody2D> targets = new List<Rigidbody2D>();
        List<Rigidbody2D> startTargetList = new List<Rigidbody2D>();

        List<RaycastHit2D> hits = new List<RaycastHit2D>();

        Quaternion baseAngle = Quaternion.AngleAxis(-_angle / 2f, Vector3.forward);
        Quaternion deltaAngle = Quaternion.AngleAxis(_angle / (_raysCount - 1) > 0 ? (_raysCount - 1) : 1, Vector3.forward);

        while (gameObject.activeSelf) {
            yield return timer;
            while (_blind) { yield return null; }

            Vector2 direction = baseAngle * _direction;
            if (_raysCount == 1) { direction = _direction; }
            for (int i = 0; i < _raysCount; i++) {
                if (Physics2D.Raycast(transform.position, direction, filter, hits, _raysDistance) > 0) {
                    for (int j = 0; j < hits.Count; j++) {
                        _onView.Invoke(hits[j]);
                        //if (!targets.Contains(hits[i].rigidbody)) {
                        //    targets.Add(hits[i].rigidbody);
                        //}
                    }
                }
                direction = deltaAngle * direction;
            }

            //startTargetList = new List<Rigidbody2D>(targets);

            //for (int i = 0; i < lastTargets.Count; i++) {
            //    if (!targets.Contains(lastTargets[i].rigidbody)) {
            //        _onEndView.Invoke(lastTargets[i]);
            //        --i;
            //    } else {
            //        startTargetList.Remove(lastTargets[i].rigidbody);
            //    }
            //}

            //for (int i = 0; i < startTargetList.Count; i++) {
            //    _onStartView(startTargetList[i]));
            //}

        }
    }

    private void OnDrawGizmosSelected() {
        if (!enabled) { return; }
        Gizmos.color = Color.blue;
        if (_blind || _raysCount <= 0) { return; }
        if (_raysCount == 1) { Gizmos.DrawRay(transform.position, _direction * _raysDistance); return; }

        Quaternion baseAngle = Quaternion.AngleAxis(-_angle/2f, Vector3.forward);
        Quaternion deltaAngle = Quaternion.AngleAxis(_angle / (_raysCount - 1), Vector3.forward);

        Vector2 direction = baseAngle * _direction;

        for (int i = 0; i < _raysCount; i++) {
            Gizmos.DrawRay(transform.position, direction * _raysDistance);
            direction = deltaAngle * direction;
        }

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _direction * _raysDistance);
    }
}
