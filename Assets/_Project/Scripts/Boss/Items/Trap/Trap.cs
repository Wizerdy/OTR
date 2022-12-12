using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;
using UnityEngine.Events;

public class Trap : MonoBehaviour {

    float _visibility = 3;
    float _effectivity = 1;
    float _tick = 1;
    int _damagesEveryTick = 1;
    bool _open = false;

    SpriteRenderer _sr;
    Animator _animator;
    EntityAbilities _playerTrapped;
    EntityPhysics _target;
    Force _force;

    [SerializeField] BetterEvent _onTrapActivation = new BetterEvent();
    public event UnityAction OnTrapActivation { add => _onTrapActivation += value; remove => _onTrapActivation -= value; }

    public Trap ChangeDamages(int damages) {
        _damagesEveryTick = damages;
        return this;
    }

    public Trap ChangeVisibility(float visibility) {
        _visibility = visibility;
        return this;
    }

    public Trap ChangeEffectivity(float effectivity) {
        _effectivity = effectivity;
        return this;
    }

    public Trap ChangeTick(float tick) {
        _tick = tick;
        return this;
    }

    private void Start() {
        _animator = GetComponentInChildren<Animator>();
        _open = false;
        _sr = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(Tools.Delay(() => { _open = true; }, _effectivity));
        StartCoroutine(Tools.Delay(() => { _sr.enabled = false; }, _visibility));
    }

    private void Update() {
        if (_playerTrapped != null) {
            _playerTrapped.transform.position = transform.position;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && _open) {
            StartCoroutine(Trapped(collision.gameObject.GetRoot().GetComponent<EntityAbilities>()));
        }
    }

    IEnumerator Trapped(EntityAbilities ea) {
        _open = false;
        _sr.enabled = true;
        _playerTrapped = ea;
        ea.Get<PlayerEntity>().Throw(ea.Get<EntityPhysics>().Velocity.normalized);
        ea.Get<EntityPhysicMovement>().InTrap();
        IHealth health = ea.transform.GetComponent<IHealth>();
        Timer timer = new Timer(this, _tick);
        timer.OnActivate += () => health.TakeDamage(_damagesEveryTick, gameObject);
        timer.Start();
        EntityIcon entityIcon = ea.Get<EntityIcon>();
        entityIcon.ShowCrossHighlight();
        _target = ea.Get<EntityPhysics>();
        _force = new Force(0, Vector2.zero, 1, Force.ForceMode.INPUT);
        _target.Add(_force, (int)PhysicPriority.BLOCK);
        _animator.SetBool("Trapped", true);
        _onTrapActivation?.Invoke();
        ea.transform.position = transform.position;
        EntityWeaponry entityWeaponry = ea.Get<EntityWeaponry>();
        while (entityWeaponry.Weapon == null) {
            yield return null;
        }
        ea.Get<EntityPhysicMovement>().OutTrap();
        entityIcon.HideCrossHighlight();
        _target.Remove(_force);
        Die();
    }

    private void OnDestroy() {
        _target?.Remove(_force);
    }

    void Die() {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
