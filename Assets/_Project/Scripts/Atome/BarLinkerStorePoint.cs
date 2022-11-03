using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class BarLinkerStorePoint : MonoBehaviour {
    [SerializeField] AtomeBar _atomeBar;
    [SerializeField] EntityStorePoint _entityStorePoint;

    private void Reset() {
        _entityStorePoint = GetComponent<EntityStorePoint>();
    }

    void Start() {
        _entityStorePoint.TooMuchPointLoseDifference += (float value) => _atomeBar.Remove(value, true);
    }

    private void OnDestroy() {
        _entityStorePoint.TooMuchPointLoseDifference += (float value) => _atomeBar.Remove(value, true);
    }
}
