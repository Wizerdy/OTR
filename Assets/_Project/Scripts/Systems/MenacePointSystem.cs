using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenacePointSystem : MonoBehaviour {
    Dictionary<EntityAbilities, int> _menaces = new Dictionary<EntityAbilities, int>();

    public void Add(EntityAbilities target, int count = 0) {
        if (!_menaces.ContainsKey(target)) {
            _menaces.Add(target, count);
        } else {
            _menaces[target] += count;
        }
        Debug.Log(target.name + " : " + _menaces[target]);
    }

    public void Remove(EntityAbilities target, int count) {
        if (!_menaces.ContainsKey(target)) { return; }

        _menaces[target] = Mathf.Max(_menaces[target] - count, 0);
    }

    public void Remove(EntityAbilities target) {
        if (!_menaces.ContainsKey(target)) { return; }

        _menaces.Remove(target);
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
}
