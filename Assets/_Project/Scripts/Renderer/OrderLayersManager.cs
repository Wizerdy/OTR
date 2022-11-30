using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderLayersManager : MonoBehaviour {
    [SerializeField] int _spacing = 5;

    List<OrderInLayerByAxis> _renderers = new List<OrderInLayerByAxis>();
    SortedList<float, List<OrderInLayerByAxis>> _sortedRenderers;

    public void Add(OrderInLayerByAxis renderer) {
        _renderers.Add(renderer);
    }

    public void Remove(OrderInLayerByAxis renderer) {
        if (!_renderers.Contains(renderer)) { return; }
        _renderers.Remove(renderer);
    }

    void Update() {
        _sortedRenderers = new SortedList<float, List<OrderInLayerByAxis>>(Comparer<float>.Create((x, y) => y.CompareTo(x)));

        for (int i = 0; i < _renderers.Count; i++) {
            float key = _renderers[i].Position.y;
            if (!_sortedRenderers.ContainsKey(key)) {
                _sortedRenderers.Add(key, new List<OrderInLayerByAxis>());
            }
            _sortedRenderers[key].Add(_renderers[i]);
        }

        int currentOder = 0;
        foreach (KeyValuePair<float, List<OrderInLayerByAxis>> list in _sortedRenderers) {
            for (int i = 0; i < list.Value.Count; i++) {
                list.Value[i].SetOrderInLayer(currentOder);
                currentOder += _spacing;
            }
        }
    }
}
