using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenacePointSystem : MonoBehaviour {
    [System.Serializable]
    class DebugStruct {
        public string name;
        public int menace;
    }

    Dictionary<EntityAbilities, int> _menaces = new Dictionary<EntityAbilities, int>();
    [SerializeField] List<DebugStruct> _debug = new List<DebugStruct>();

    public void Add(EntityAbilities target, int count = 0) {
        if (!_menaces.ContainsKey(target)) {
            _menaces.Add(target, count);
            _debug.Add(new DebugStruct());
            _debug[^1].name = target.name;
            _debug[^1].menace = count;
        } else {
            _menaces[target] += count;
            _debug[DebugFind(target.name)].menace += count;
        }
        //Debug.Log(target.name + " : " + _menaces[target]);
    }

    public void Remove(EntityAbilities target, int count) {
        if (!_menaces.ContainsKey(target)) { return; }

        _debug[DebugFind(target.name)].menace = Mathf.Max(_menaces[target] - count, 0);
        _menaces[target] = Mathf.Max(_menaces[target] - count, 0);
    }

    public void Remove(EntityAbilities target) {
        if (!_menaces.ContainsKey(target)) { return; }

        _menaces.Remove(target);
    }

    public void Set(EntityAbilities target, int count) {
        _debug[DebugFind(target.name)].menace = count;
        _menaces[target] = count;
    }

    public int? Get(EntityAbilities target) {
        if (!_menaces.ContainsKey(target)) { return null; }

        return _menaces[target];
    }

    public EntityAbilities Threatening() {
        if (_menaces.Count == 0) { return null; }

        EntityAbilities output = null;
        int max = -1;

        foreach (KeyValuePair<EntityAbilities, int> menace in _menaces) {
            if (max <= menace.Value) {
                output = menace.Key;
                max = menace.Value;
            }
        }
        return output;
    }

    private int DebugFind(string name) {
        for (int i = 0; i < _debug.Count; i++) {
            if (_debug[i].name.Equals(name)) {
                return i;
            }
        }
        return -1;
    }
}
