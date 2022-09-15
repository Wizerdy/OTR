using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class EntityAttacks : MonoBehaviour, IEntityAbility {
    [SerializeField] Transform _root;
    [SerializeField] List<Named<Attack>> _attackList;

    public Attack Get(string name) {
        for (int i = 0; i < _attackList.Count; i++) {
            if (_attackList[i].name == name) {
                return _attackList[i].value;
            }
        }
        return null;
    }

    public Coroutine Use(string name, GameObject target, Vector2 direction) {
        return Use(Get(name), target, direction);
    }

    public Coroutine Use(Attack attack, GameObject target, Vector2 direction) {
        return StartCoroutine(attack.Use(_root.gameObject, target, direction));
    }
}
