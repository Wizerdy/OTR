using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class EntityRevive : MonoBehaviour, IEntityAbility {
    [SerializeField] Health _health;
    [SerializeField] Collider2D _collider;

    [Header("System")]
    [SerializeField] float _reviveTime = 0.2f;
    [SerializeField] float _radius = 5f;
    [SerializeField] int _pressCount = 10;
    [SerializeField, Range(0f, 1f)] float _healthPercentage = 0.5f;

    [SerializeField, HideInInspector] BetterEvent<EntityRevive> _onStartRevive = new BetterEvent<EntityRevive>();
    [SerializeField, HideInInspector] BetterEvent<EntityRevive> _onStopRevive = new BetterEvent<EntityRevive>();

    int _currentPress;
    bool _revivingSomeone = false;

    public float ReviveTime => _reviveTime;

    public event UnityAction<EntityRevive> OnStartRevive { add => _onStartRevive += value; remove => _onStartRevive -= value; }
    public event UnityAction<EntityRevive> OnStopRevive { add => _onStopRevive += value; remove => _onStopRevive -= value; }

    void Start() {
        _health.OnDeath += _Death;
    }

    void _Death() {
        _currentPress = 0;
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

    public bool CheckRevive(out EntityRevive target) {
        target = OverlapNearestRevive();
        if (target == null) { return false; }
        if (_revivingSomeone) { return false; }
        target.HeartMassage();
        _revivingSomeone = true;
        StartCoroutine(Tools.Delay(() => _revivingSomeone = false, _reviveTime));
        return true;
    }

    private EntityRevive OverlapNearestRevive() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius);
        if (colliders == null || colliders.Length == 0) { return null; }

        EntityRevive output = null;
        float nearest = _radius * _radius + 1f;
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i] == _collider) { continue; }
            if (!colliders[i].CompareTag("Player")) { continue; }
            EntityRevive other = colliders[i].gameObject.GetRoot().GetComponent<EntityAbilities>()?.Get<EntityRevive>() ?? null;
            if (other == null) { continue; }
            float sqrDistance = Vector2.SqrMagnitude(other.transform.Position2D() - transform.Position2D());
            if (nearest > sqrDistance) {
                output = other;
                nearest = sqrDistance;
            }
        }
        return output;
    }

    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
