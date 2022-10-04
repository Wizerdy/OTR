using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAbilities : MonoBehaviour {
    List<IEntityAbility> _abilities = new List<IEntityAbility>();

    public T Get<T>() where T : MonoBehaviour, IEntityAbility {
        for (int i = 0; i < _abilities.Count; i++) {
            if (_abilities[i] is T ability) {
                return ability;
            }
        }

        return Add<T>();
    }

    private T Add<T>() where T : MonoBehaviour, IEntityAbility {
        T ability = GetComponentInChildren<T>();
        if (_abilities == null) { return null; }
        _abilities.Add(ability);
        return ability;
    }
}
