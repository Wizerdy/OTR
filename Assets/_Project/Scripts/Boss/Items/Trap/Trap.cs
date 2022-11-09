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
        _open = true;
        _sr = GetComponent<SpriteRenderer>();
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
        EntityPhysics entityPhysics = ea.Get<EntityPhysics>();
        IHealth health = ea.transform.GetComponent<IHealth>();
        timer.OnActivate += () => health.TakeDamage(_damagesEveryTick, gameObject);
        timer.Start();
        Force force = new Force(0, Vector2.zero, 1, Force.ForceMode.INPUT);
        entityPhysics.Add(force, (int)PhysicPriority.BLOCK);
        while (entityWeaponry.Weapon == null) {
            yield return null;
        }
        entityPhysics.Remove(force);
        Die();
    }

    void Die() {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
