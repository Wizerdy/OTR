using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrajectoryLine : MonoBehaviour
{
    [SerializeField] private float length = 3.0f;
    [SerializeField] private float startThiccness = 0.7f;
    [SerializeField] private float endThiccness = 0.3f;
    [SerializeField] private PlayerEntity entity;
    private LineRenderer lineRenderer;
    private Vector2 playerDir;
    private Vector2 rayDir;
    private int linePoint = 0;
    private ContactFilter2D filter = new ContactFilter2D();
    // Start is called before the first frame update
    void Start()
    {
        if (entity == null) { return; }
        entity.OnAim += GetDirection;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.startWidth = startThiccness;
        lineRenderer.endWidth = endThiccness;
        filter.useTriggers = false;
        Physics2D.queriesHitTriggers = false;
    }

    // Update is called once per frame
    void Update()
    {
        ShootingRay(transform.position);

        linePoint = 0;
    }

    private void ShootingRay(Vector2 pos) {
        //linePoint++;


        //List<RaycastHit2D> hits = Physics2D.Raycast(transform.position, playerDir, filter, length);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, playerDir, length) ;
        Vector2 endPos = pos + (length * playerDir);
        Debug.Log(hit.transform.name);
        if (hit) {
            //endPos = hit.transform.position;
            //Vector2 bouncePos = hit.transform.position;
            //lineRenderer.SetPosition(linePoint, bouncePos);
            //linePoint++;
        }

        lineRenderer.SetPosition(0, pos);
        lineRenderer.SetPosition(1, endPos);
    }

    private void GetDirection(Vector2 direction) {
        playerDir = direction;
    }

    private void OnDestroy() {
        if (entity != null) { return; }
        entity.OnAim -= GetDirection;
    }
}
