using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class Trap : MonoBehaviour {
    [SerializeField] Sprite closeSprite;
    SpriteRenderer _sr;
    float _visibility;
    int _damagesEveryTick;
    float _tick;
    bool _open;
    Animator _animator;

    EntityPhysics _target;
    Force _force;

    public Trap ChangeDamages(int damages) {
        _damagesEveryTick = damages;
        return this;
    }

    public Trap ChangeVisibility(float visibility) {
        _visibility = visibility;
        return this;
    }

    public Trap ChangeTick(float tick) {
        _tick = tick;
        return this;
    }

    private void Start() {
        _animator = GetComponentInChildren<Animator>();
        _open = true;
        _sr = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(Tools.Delay(() => {
            _sr.enabled = false;
            _open = false;
        }, _visibility));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player") && !_open) {
            StartCoroutine(Trapped(collision.gameObject.GetRoot().GetComponent<EntityAbilities>()));
        }
    }

    IEnumerator Trapped(EntityAbilities ea) {
        _open = false;
        _sr.enabled = true;
        _sr.sprite = closeSprite;
        Timer timer = new Timer(this, _tick);
        _open = true;
        ea.Get<PlayerEntity>().Throw(ea.Get<EntityPhysics>().Velocity.normalized);
        EntityWeaponry entityWeaponry = ea.Get<EntityWeaponry>();
        EntityIcon entityIcon = ea.Get<EntityIcon>();
        entityIcon.ShowCrossHighlight();
        _target = ea.Get<EntityPhysics>();
        IHealth health = ea.transform.GetComponent<IHealth>();
        timer.OnActivate += () => health.TakeDamage(_damagesEveryTick, gameObject);
        timer.Start();
        _force = new Force(0, Vector2.zero, 1, Force.ForceMode.INPUT);
        _target.Add(_force, (int)PhysicPriority.BLOCK);
        ea.transform.position = transform.position;
        _animator.SetBool("Trapped", true);
        while (entityWeaponry.Weapon == null) {
            yield return null;
        }
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
