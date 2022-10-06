using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrajectoryLine : MonoBehaviour {
    [SerializeField] private float length = 3.0f;
    [SerializeField] private float width = 1.0f;
    [SerializeField] private AnimationCurve _curve = new AnimationCurve();
    [SerializeField] private float reflectionOffSet = 0.15f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask ignoreMask;

    private Vector2 playerDir;
    private Vector2 originalDir;

    public Vector2 Direction { get => playerDir; set => playerDir = value; }

    void Start() {
        //lineRenderer.startWidth = startThiccness;
        //lineRenderer.endWidth = endThiccness;
        lineRenderer.widthMultiplier = width;
        lineRenderer.widthCurve = _curve;
        Physics2D.queriesHitTriggers = false;
        Physics2D.queriesStartInColliders = false;
    }

    void Update() {
        ShootTargetingLine();
    }

    public void ShootTargetingLine() {

        // recalcul ray if player move or change direction
        if (transform.position != lineRenderer.GetPosition(0) || originalDir != playerDir) {
            ResetLine();
            ShootRay(length, playerDir, transform.position);
        }  else {
            // if no change
        }
    }

    private void ResetLine() {
        originalDir = playerDir;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);
    }

    private void ShootRay(float distance, Vector2 direction, Vector2 startPos) {
        //Debug.Log("NUM "+ lineRenderer.positionCount +"     DIS" + distance);
        if (distance <= 0.0f) {
            return;
        }

        lineRenderer.positionCount++;

        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, distance, ~ignoreMask);
        //Debug.Log("LINE NUM " + lineRenderer.positionCount + "        DIR " + direction + "     DISTANCE " + distance + "     START POS" + spawnPos);
        if (hit.point != Vector2.zero) {
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
            float distanceLeft = distance - Vector2.Distance(startPos, hit.point);
            Vector2 newDir = RayBounce(direction, hit.normal) * reflectionOffSet;
            ShootRay(distanceLeft,  newDir, hit.point);
        } else {
            Vector2 endPos = startPos + distance * direction.normalized;
            //Debug.Log("START " + startPos +"    END " + endPos + "    DISTANCE "+distance + "     DIRECTION " + direction);
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, endPos);
        }
    }

    private Vector2 RayBounce(Vector2 dir, Vector2 normal) {
        return Vector2.Reflect(dir, normal);
    }
}
