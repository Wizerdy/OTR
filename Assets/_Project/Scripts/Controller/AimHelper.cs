using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class AimHelper : MonoBehaviour {
    [System.Serializable]
    class AimableTarget {
        public Transform target;
        public float angle;

        public AimableTarget(Transform t, float a) {
            target = t;
            angle = a;
        }

        public override string ToString() {
            return "{ " + target.name + ", " + angle + " }";
        }
    }

    [SerializeField] List<AimableTarget> _targetsList;

    public Vector2 Aim(Vector2 position, Vector2 direction) {
        if (_targetsList.Count == 0) { return direction; }

        Vector2 output = direction;
        float minAngle = Mathf.Infinity;
        float deltaAngle;
        for (int i = 0; i < _targetsList.Count; i++) {
            //if (!_targetsList[i].target.IsValid()) { continue; }
            if (_targetsList[i].target.Position2D() == position) { continue; }
            Vector2 targetDirection = (_targetsList[i].target.Position2D() - position).normalized;
            deltaAngle = Vector2.Angle(direction, targetDirection);

            if (_targetsList[i].angle / 2f < deltaAngle) { continue; }
            if (minAngle > deltaAngle) {
                minAngle = deltaAngle;
                output = targetDirection;
            }
        }

        return output;
    }

    public void Add(Transform transform, float angle) {
        if (Contains(transform)) { Debug.LogWarning("Already contained : " + transform.name); return; }
        _targetsList.Add(new AimableTarget(transform, angle));
        Debug.Log("Added To List : " + _targetsList[^1].target.name + " .. " + _targetsList.Count);
    }

    private AimableTarget Find(Transform transform) {
        for (int i = 0; i < _targetsList.Count; i++) {
            if (_targetsList[i].target == transform) {
                return _targetsList[i];
            }
        }
        return null;
    }

    public bool Contains(Transform transform) {
        return Find(transform) != null;
    }

    public void Remove(Transform transform) {
        AimableTarget target = Find(transform);
        if (target == null) { Debug.LogWarning("Transform not found : " + transform.name); return; }

        _targetsList.Remove(target);
    }
}
