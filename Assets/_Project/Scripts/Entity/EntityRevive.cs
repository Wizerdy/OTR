using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;

public class EntityRevive : MonoBehaviour, IEntityAbility {
    [SerializeField] Health _health;
    [SerializeField] EntityPhysics _physics;
    [SerializeField] Collider2D _collider;
    [SerializeField] Slider _reviveSlider;

    [Header("System")]
    [SerializeField] float _reviveTime = 0.5f;
    [SerializeField] float _heartMassageTime = 0.2f;
    [SerializeField] float _radius = 5f;
    [SerializeField] int _pressCount = 10;
    [SerializeField, Range(0f, 1f)] float _healthPercentage = 0.5f;

    [SerializeField] BetterEvent<EntityRevive> _onStartRevive = new BetterEvent<EntityRevive>();
    [SerializeField, HideInInspector] BetterEvent<EntityRevive> _onStopRevive = new BetterEvent<EntityRevive>();

    int _currentPress;
    bool _revivingSomeone = false;

    public float HeartMassageTime => _heartMassageTime;

    public event UnityAction<EntityRevive> OnStartRevive { add => _onStartRevive += value; remove => _onStartRevive -= value; }
    public event UnityAction<EntityRevive> OnStopRevive { add => _onStopRevive += value; remove => _onStopRevive -= value; }

    void Start() {
        _health.OnDeath += _Death;
    }

    void _Death() {
        _currentPress = 0;
        _collider.isTrigger = true;
        _physics.Terminate();
        _reviveSlider.gameObject.SetActive(true);
        UpdateSlider();
    }

    public bool HeartMassage() {
        ++_currentPress;
        UpdateSlider();
        if (_currentPress >= _pressCount) {
            _currentPress = 0;
            _reviveSlider.gameObject.SetActive(false);
            _onStartRevive.Invoke(this);
            StartCoroutine(Tools.Delay(Revive, _reviveTime));
            return true;
        }
        return false;
    }

    private void UpdateSlider() {
        _reviveSlider.value = (float)_currentPress / (float)_pressCount;
    }

    public void Revive() {
        _collider.isTrigger = false;
        _health.TakeHeal(_healthPercentage);
        _onStopRevive.Invoke(this);
    }

    public bool CheckRevive(out EntityRevive target) {
        target = OverlapNearestRevive();
        if (target == null) { return false; }
        if (_revivingSomeone) { return false; }
        target.HeartMassage();
        _revivingSomeone = true;
        StartCoroutine(Tools.Delay(() => _revivingSomeone = false, _heartMassageTime));
        return true;
    }

    private EntityRevive OverlapNearestRevive() {
        bool querryTrigger = Physics2D.queriesHitTriggers;
        Physics2D.queriesHitTriggers = true;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _radius);
        Physics2D.queriesHitTriggers = querryTrigger;
        if (colliders == null || colliders.Length == 0) { return null; }

        EntityRevive output = null;
        float nearest = _radius * _radius + 1f;
        for (int i = 0; i < colliders.Length; i++) {
            if (colliders[i] == _collider) { continue; }
            if (!colliders[i].CompareTag("Player")) { continue; }
            GameObject root = colliders[i].gameObject.GetRoot();
            if (!root.GetComponent<IHealth>().IsDead) { continue; }
            EntityRevive other = root.GetComponent<EntityAbilities>()?.Get<EntityRevive>() ?? null;
            if (other == null) { continue; }
            float sqrDistance = Vector2.SqrMagnitude(root.transform.Position2D() - transform.Position2D());
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
