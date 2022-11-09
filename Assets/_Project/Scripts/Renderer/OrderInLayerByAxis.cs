using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class OrderInLayerByAxis : MonoBehaviour {
    [SerializeField] Axis2D _axis = Axis2D.Y;
    [SerializeField] Vector2 _positionOffset = Vector2.zero;

    Renderer[] _renderers;

    void Start() {
        FindRenderers();
    }

    void Update() {
        UpdateRenderers();
    }

    public void UpdateRenderers() {

    }

    public void FindRenderers() {
        _renderers = GetComponents<Renderer>();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.Position2D() + _positionOffset, 1f);
    }
}
