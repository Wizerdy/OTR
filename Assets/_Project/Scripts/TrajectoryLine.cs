using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrajectoryLine : MonoBehaviour {
    [SerializeField] private float length = 3.0f;
    [SerializeField] private float startThiccness = 0.7f;
    [SerializeField] private float endThiccness = 0.3f;
    [SerializeField] private PlayerEntity entity;
    [SerializeField] private float reflectionOffSet = 0.15f;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private LayerMask ignoreMask;
    private Vector2 playerDir;
    private Vector2 originalDir;

    // Start is called before the first frame update
    void Start() {
        if (entity == null) { return; }
        entity.OnAim += GetDirection;

        lineRenderer.startWidth = startThiccness;
        lineRenderer.endWidth = endThiccness;
        Physics2D.queriesHitTriggers = false;
        Physics2D.queriesStartInColliders = false;
    }

    // Update is called once per frame
    void Update() {
        ShootTargetingLine();
    }

    private void ShootTargetingLine() {

        // recalcul ray if player move or change direction
        if (entity.transform.position != lineRenderer.GetPosition(0) || originalDir != playerDir) {
            ResetLine();
            ShootRay(length, playerDir, entity.transform.position);
        }  else {
            // if no change
        }
    }

    private void ResetLine() {
        originalDir = playerDir;
        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, entity.transform.position);
    }

    private void ShootRay(float distance, Vector2 direction, Vector2 startPos) {
        //Debug.Log("NUM "+ lineRenderer.positionCount +"     DIS" + distance);
        if (distance <= 0.0f) {
            return;
        }

        lineRenderer.positionCount++;

        RaycastHit2D hit = Physics2D.Raycast(startPos, direction, distance, ignoreMask);
        //Debug.Log("LINE NUM " + lineRenderer.positionCount + "        DIR " + direction + "     DISTANCE " + distance + "     START POS" + spawnPos);
        if (hit.point != Vector2.zero) {
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, hit.point);
            float distanceLeft = distance - Vector2.Distance(startPos, hit.point);
            Vector2 newDir = RayBounce(direction, hit.normal) * reflectionOffSet * -1;
            ShootRay(distanceLeft,  newDir, hit.point);
        } else {
            Vector2 endPos = startPos + distance * direction.normalized;
            //Debug.Log("START " + startPos +"    END " + endPos + "    DISTANCE "+distance + "     DIRECTION " + direction);
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, endPos);
        }
    }

    private void GetDirection(Vector2 direction) {
        playerDir = direction;
    }

    private Vector2 RayBounce(Vector2 dir, Vector2 normal) {
        return Vector2.Reflect(dir, normal) * -1;
    }

    private void OnDestroy() {
        if (entity != null) { return; }
        entity.OnAim -= GetDirection;
    }

}
