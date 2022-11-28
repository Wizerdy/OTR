using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;

public class ForAllerParasite : MonoBehaviour {
    [SerializeField] Sprite _damageSprite;
    [SerializeField] List<ForAllerParasite> _parasites;
    [SerializeField] float _timeVisible = 0.5f;
    float _timeBeforeActivation;
    IHealth _host;
    Vector2 _distMinMax;
    Vector2 _damagesVector;
    bool _damageDone;
    SpriteRenderer _sr;

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
        _sr = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        StartCoroutine(Tools.Delay(() => Parasited(), _timeBeforeActivation));
    }

    void Parasited() {
        if (_damageDone) return;
        float dist = 0;
        int damages;
        for (int i = 0; i < _parasites.Count; i++) {
            if (_parasites.Count == 1) {
                dist += _damagesVector.x;
            } else if(i == 0){
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
        transform.GetChild(0).gameObject.GetComponent<Animator>().SetTrigger("Spark");
        _sr.sprite = _damageSprite;
        _damageDone = true;
        _host.TakeDamage(damages, gameObject); 
    }
}
