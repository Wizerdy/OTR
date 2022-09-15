using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAbilities : MonoBehaviour {
    List<IEntityAbility> _abilities = new List<IEntityAbility>();

    public T Get<T>() where T : MonoBehaviour {
        for (int i = 0; i < _abilities.Count; i++) {
            if (_abilities[i] is T) {
                return (T)_abilities[i];
            }
        }

        return Add<T>();
    }

    private T Add<T>() where T : MonoBehaviour {
        T ability = GetComponentInChildren<T>();
        if (_abilities == null || !(ability is IEntityAbility)) { return null; }
        _abilities.Add((IEntityAbility)ability);
        return ability;
    }
}
