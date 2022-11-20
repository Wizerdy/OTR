using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour {
    [SerializeField] Sprite _impactSprite;
    SpriteRenderer _sr;
    int _damages;
    float _timeBeforeFall;
    bool _impact = false;
    List<IHealth> _hit = new List<IHealth>();
    public Meteor ChangeDamages(int damages) {
        _damages = damages;
        return this;
    }

    public Meteor ChangeTimeBeforeFall(float timeBeforeFall) {
        _timeBeforeFall = timeBeforeFall;
        return this;
    }

    void Start() {
        _sr = GetComponentInChildren<SpriteRenderer>();
        StartCoroutine(Impact());
    }


    private void OnTriggerStay2D(Collider2D collision) {
        Debug.Log(collision.gameObject.GetRoot());
        IHealth health = collision.gameObject.GetRoot().GetComponent<IHealth>();
        if (health != null && !_hit.Contains(health) && collision.CompareTag("Player") && _impact) {
            health.TakeDamage(_damages, gameObject);
            _hit.Add(health);
        }
    }

    IEnumerator Impact() {
        yield return new WaitForSeconds(_timeBeforeFall);
        _impact = true;
        _sr.sprite = _impactSprite;
        yield return new WaitForSeconds(0.5f);
        Debug.Log("/////////////"+gameObject);
        Die();
    }

    void Die() {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
