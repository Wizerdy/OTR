using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using ToolsBoxEngine.BetterEvents;
using UnityEngine.Events;

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
    [SerializeField] BetterEvent _onSpark = new BetterEvent();
    public event UnityAction OnSpark { add => _onSpark += value; remove => _onSpark -= value; }

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
        //float dist = Mathf.Infinity;
        //int parasite = 0;
        //for (int i = 0; i < _parasites.Count; i++) {
        //    if (_parasites[i] != this && _distMinMax.x < Vector3.Distance(_parasites[i].transform.position, transform.position) && dist > Vector3.Distance(_parasites[i].transform.position, transform.position)) {
        //        dist = Vector3.Distance(_parasites[i].transform.position, transform.position);
        //        parasite = i;
        //    }
        //}
        //Quaternion rotation;
        //if (dist == Mathf.Infinity) {
        //    _arrowGraphics.sprite = _checkSprite; 
        //    rotation = Quaternion.Euler(0f, 0f, -90f);
        //} else {
        //    if (_parasites[parasite].gameObject.transform.position.x < _arrow.transform.position.x) {
        //        rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, _parasites[parasite].gameObject.transform.position - _arrow.transform.position));
        //    } else {
        //        rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.up, _parasites[parasite].gameObject.transform.position - _arrow.transform.position));
        //    }
        //    _arrowGraphics.sprite = _arrowSprite;
        //}
        //_arrow.transform.rotation = rotation;
        (bool, ForAllerParasite, float) datas = ComputeDatas();

        Quaternion rotation;
        if (datas.Item1) {
            _arrowGraphics.sprite = _checkSprite;
            rotation = Quaternion.Euler(0f, 0f, -90f);
        } else {
            if (datas.Item2.gameObject.transform.position.x < _arrow.transform.position.x) {
                rotation = Quaternion.Euler(0, 0, Vector3.Angle(Vector3.up, datas.Item2.gameObject.transform.position - _arrow.transform.position));
            } else {
                rotation = Quaternion.Euler(0, 0, -Vector3.Angle(Vector3.up, datas.Item2.gameObject.transform.position - _arrow.transform.position));
            }
            _arrowGraphics.sprite = _arrowSprite;
        }
        _arrow.transform.rotation = rotation;

    }

    void Parasited() {
        if (_damageDone) return;
        //float dist = 0;
        //int damages;
        //for (int i = 0; i < _parasites.Count; i++) {
        //    if (_parasites.Count == 1) {
        //        dist += _damagesVector.x;
        //    } else if (i == 0) {
        //        dist += Vector3.Distance(_parasites[i].transform.position, _parasites[_parasites.Count - 1].transform.position);
        //    } else {
        //        dist += Vector3.Distance(_parasites[i - 1].transform.position, _parasites[i].transform.position);
        //    }
        //}
        //dist /= _parasites.Count;
        //damages = (int)Mathf.Lerp(_damagesVector.x, _damagesVector.y, Mathf.InverseLerp(_distMinMax.x, _distMinMax.y, dist));
        //for (int i = 0; i < _parasites.Count; i++) {
        //    _parasites[i].DoDamage(damages);
        //}
        (bool, ForAllerParasite, float) datas = ComputeDatas();

        if (datas.Item1) {
            DoDamage(0);
            return;
        }

        DoDamage(Mathf.RoundToInt(Tools.Remap(datas.Item3, _distMinMax.x, _distMinMax.y, _damagesVector.x, _damagesVector.y)));
    }

    public (bool, ForAllerParasite, float) ComputeDatas() {
        float dist = Mathf.Infinity;
        int parasite = 0;
        bool fine = false;
        for (int i = 0; i < _parasites.Count; i++) {
            if (_parasites[i] == this) { continue; }
            float distance = Vector3.Distance(_parasites[i].transform.position, transform.position);
            if (distance < _distMinMax.x) {
                fine = true;
                break;
            }
            if (dist > distance) {
                dist = distance;
                parasite = i;
            }
        }

        return new(fine, _parasites[parasite], dist);
    }

    public void DoDamage(int damages) {
        if (damages <= 0) {
            _arrowGraphics.gameObject.SetActive(false);
            _damageDone = true;
            return;
        }
        _onSpark?.Invoke();
        _animator.SetTrigger("Spark");
        _host.TakeDamage(damages, gameObject);
    }
}
