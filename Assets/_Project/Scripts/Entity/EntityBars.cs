using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class EntityBars : MonoBehaviour, IEntityAbility {
    [SerializeField] List<AtomeBar> _atomeBars = new List<AtomeBar>();
    public Dictionary<string, AtomeBar> _bars;
    private void Awake() {
        _bars = new Dictionary<string, AtomeBar>();
    }

    private void Start() {
        for (int i = 0; i < _atomeBars.Count; i++) {
            _bars.Add(_atomeBars[i].gameObject.name, _atomeBars[i]);
        }
    }
}
