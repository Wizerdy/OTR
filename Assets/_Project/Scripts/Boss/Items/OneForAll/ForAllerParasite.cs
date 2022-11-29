using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class ForAllerParasite : MonoBehaviour {
    [SerializeField] List<ForAllerParasite> _parasites;
    [SerializeField] Sprite _arrowSprite;
    [SerializeField] Sprite _checkSprite;
    [SerializeField] GameObject _arrow;
    [SerializeField] SpriteRenderer _arrowGraphics;
    [SerializeField] Animator _animator;
    float _timeBeforeActivation;
    IHealth _host;
    Vector2 _distMinMax;
    Vector2 _damagesVector;
    bool _damageDone;

    public ForAllerParasite ChangeTimeBeforeActivation(float newTime) {
        _timeBeforeActivation = newTime;
        return this;
    }

    public ForAllerParasite ChangeHost(IHealth newHost) {
        _host = newHost;
        return this;
    }

    public ForAllerParasite ChangeParasite(List<ForAllerParasite> parasites) {
        _parasites = parasites;
        return this;
    }

    public ForAllerParasite ChangeDist(Vector2 distMinMax) {
        _distMinMax = distMinMax;
        return this;
    }

    public ForAllerParasite ChangeDamages(Vector2 damagesVector) {
        _damagesVector = damagesVector;
        return this;
    }



    private void Start() {
        StartCoroutine(Tools.Delay(() => Parasited(), _timeBeforeActivation));
    }

    private void Update() {
        float dist = 10000f;
        int parasite = 0;
        for (int i = 0; i < _parasites.Count; i++) {
            if (_parasites[i] != this && _distMinMax.x < Vector3.Distance(_parasites[i].transform.position, transform.position) && dist > Vector3.Distance(_parasites[i].transform.position, transform.position)) {
                dist = Vector3.Distance(_parasites[i].transform.position, transform.position);
                parasite = i;
            }
        }
        Quaternion rotation;
        if (dist == 10000f) {
            _arrowGraphics.sprite = _checkSprite; 
            rotation = Quaternion.Euler(0, 0, 180);
        } else {
            if (_parasites[parasite].gameObject.transform.position.x < _arrow.transform.position.x) {
                rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, _parasites[parasite].gameObject.transform.position - _arrow.transform.position));
            } else {
                rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.up, _parasites[parasite].gameObject.transform.position - _arrow.transform.position));
            }
            _arrowGraphics.sprite = _arrowSprite;
        }
        _arrow.transform.rotation = rotation;
    }

    void Parasited() {
        if (_damageDone) return;
        float dist = 0;
        int damages;
        for (int i = 0; i < _parasites.Count; i++) {
            if (_parasites.Count == 1) {
                dist += _damagesVector.x;
            } else if (i == 0) {
                dist += Vector3.Distance(_parasites[i].transform.position, _parasites[_parasites.Count - 1].transform.position);
            } else {
                dist += Vector3.Distance(_parasites[i - 1].transform.position, _parasites[i].transform.position);
            }
        }
        dist /= _parasites.Count;
        damages = (int)Mathf.Lerp(_damagesVector.x, _damagesVector.y, Mathf.InverseLerp(_distMinMax.x, _distMinMax.y, dist));
        for (int i = 0; i < _parasites.Count; i++) {
            _parasites[i].DoDamage(damages);
        }
    }

    public void DoDamage(int damages) {
        _arrowGraphics.gameObject.SetActive(false);
        _animator.SetTrigger("Spark");
        _damageDone = true;
        _host.TakeDamage(damages, gameObject);
    }
}
