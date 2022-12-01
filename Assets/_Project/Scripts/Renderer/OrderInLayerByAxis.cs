using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class OrderInLayerByAxis : MonoBehaviour {
    [SerializeField] Vector2 _positionOffset = Vector2.zero;

    static OrderLayersManager _manager;

    Renderer[] _renderers;

    public Vector2 Position => transform.Position2D() + _positionOffset;

    void Start() {
        FindRenderers();
        FindManager();
        _manager.Add(this);
    }

    private void OnDestroy() {
        _manager?.Remove(this);
    }

    public void SetOrderInLayer(int layer) {
        for (int i = 0; i < _renderers.Length; i++) {
            _renderers[i].sortingOrder = layer;
        }
    }

    public void FindRenderers() {
        _renderers = GetComponents<Renderer>();
    }

    public void FindManager() {
        if (_manager != null) { return; }
        _manager = FindObjectOfType<OrderLayersManager>();
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.Position2D() + _positionOffset, 0.1f);
    }
}
