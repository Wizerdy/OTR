using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class EntityRevive : MonoBehaviour, IEntityAbility {
    [SerializeField] EntityCollisionArea _reviveArea;
    [SerializeField] Health _health;
    [SerializeField] Collider2D _collider;

    [Header("System")]
    [SerializeField] int _pressCount = 10;
    [SerializeField, Range(0f, 1f)] float _healthPercentage = 0.5f;

    [SerializeField, HideInInspector] BetterEvent<EntityRevive> _onStartRevive = new BetterEvent<EntityRevive>();
    [SerializeField, HideInInspector] BetterEvent<EntityRevive> _onStopRevive = new BetterEvent<EntityRevive>();

    int _currentPress;
    EntityRevive _reviving = null;

    public bool Reviving => _reviving != null;
    public EntityRevive Target => _reviving;

    public event UnityAction<EntityRevive> OnStartRevive { add => _onStartRevive += value; remove => _onStartRevive -= value; }
    public event UnityAction<EntityRevive> OnStopRevive { add => _onStopRevive += value; remove => _onStopRevive -= value; }

    void Start() {
        _health.OnDeath += _Death;
    }

    void _Death() {
        _collider.isTrigger = true;
    }

    public bool HeartMassage() {
        ++_currentPress;
        if (_currentPress >= _pressCount) {
            _currentPress = 0;
            Revive();
            return true;
        }
        return false;
    }

    public void Revive() {
        _collider.isTrigger = false;
        _health.TakeHeal(_healthPercentage);
    }

    public bool CheckRevive() {
        if (_reviving == null) {
            GameObject obj = _reviveArea?.Nearest((KeyValuePair<GameObject, Token> pair) => { if (pair.Key == null) { return false; } return pair.Key?.CompareTag("Player") ?? false && (pair.Key?.GetComponentInRoot<IHealth>()?.IsDead ?? false); } );
            if (obj != null) {
                StartRevive(obj.GetComponentInRoot<EntityAbilities>().Get<EntityRevive>());
            } else {
                return false;
            }
        }

        if (_reviving.HeartMassage()) {
            StopRevive();
            return false;
        }

        if (!_reviving._health.IsDead) { StopRevive(); }
        return true;
    }

    public void StartRevive(EntityRevive revive) {
        _reviving = revive;
        _onStartRevive.Invoke(revive);
    }

    public void StopRevive() {
        _onStopRevive.Invoke(_reviving);
        _reviving = null;
    }
}
