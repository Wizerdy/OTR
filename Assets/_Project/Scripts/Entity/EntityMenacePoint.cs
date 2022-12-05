using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;

public class EntityMenacePoint : MonoBehaviour, IEntityAbility {
    [SerializeField] MenacePointSystemReference _menacePointSystem;
    [SerializeField] EntityAbilities _entityAbilities;

    public int Menace => _menacePointSystem?.Instance?.Get(_entityAbilities) ?? -1;

    private void Start() {
        if (_menacePointSystem.IsValid()) {
            _menacePointSystem.Instance.Add(_entityAbilities);
        }
    }

    private void OnDestroy() {
        if (_menacePointSystem.IsValid()) {
            _menacePointSystem.Instance.Remove(_entityAbilities);
        }
    }

    public void Add(int count) {
        if (!_menacePointSystem.IsValid()) { return; }
        _menacePointSystem.Instance.Add(_entityAbilities, count);
    }

    public void Remove(int count) {
        if (!_menacePointSystem.IsValid()) { return; }
        _menacePointSystem.Instance.Remove(_entityAbilities, count);
    }

    public void Set(int count) {
        if (!_menacePointSystem.IsValid()) { return; }
        _menacePointSystem.Instance.Set(_entityAbilities, count);
    }
}
