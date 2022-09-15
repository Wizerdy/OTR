using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sloot;
using static UnityEditor.Experimental.GraphView.GraphView;
using static UnityEngine.GraphicsBuffer;

public class Coda : MonoBehaviour {
    [SerializeField] int health;
    [SerializeField] List<Transform> _sensiblePosition = new List<Transform>();
    [SerializeField] List<SensiblePoint> _sensiblePointList = new List<SensiblePoint>();
    [SerializeField] SensiblePoint _sensiblePrefab;
    [SerializeField] LaserCoda _laser;
    [SerializeField] LineRenderer _llaser;
    [SerializeField] float _timeBetweenAttack;
    [SerializeField] float _teleportDist;
    Timer _laserShoot;
    [SerializeField] float _laseraim;
    [SerializeField] float _laserwait;
    [SerializeField] float _lasershot;
    [SerializeField] Gradient g1;
    [SerializeField] Gradient g2;

    void Start() {
        _llaser = _laser.GetComponent<LineRenderer>();
        _laser.transform.parent = null;
        for (int i = 0; i < _sensiblePosition.Count; i++) {
            _sensiblePointList.Add(Instantiate(_sensiblePrefab, transform));
        }
        for (int i = 0; i < _sensiblePosition.Count; i++) {
            _sensiblePointList[i].transform.localPosition = _sensiblePosition[i].localPosition;
            _sensiblePointList[i].Touche += Hit;
        }
        NewSensiblePoint();
        _laserShoot = new Timer(this, _timeBetweenAttack);
        _laserShoot.OnActivate += () => StartCoroutine(Laser());
        _laserShoot.Start();
    }

    void NewSensiblePoint() {
        int random = Random.Range(0, _sensiblePosition.Count);
        for (int i = 0; i < _sensiblePosition.Count; i++) {
            if(i == random) {
                _sensiblePointList[i].SensibleState = true;
            }
        }
    }

    IEnumerator Laser() {
        _laserShoot.Pause();
        yield return StartCoroutine(Aim());
        _laser.transform.position = FindObjectOfType<PlayerEntity>().transform.position;
        _laser.transform.right = transform.position - _laser.transform.position;
        yield return new WaitForSeconds(_laserwait);
        yield return StartCoroutine(Shoot());
        _laserShoot.Start();
    }

    IEnumerator Aim() {
        _llaser.colorGradient = g1;
        float time = _laseraim;
        float width = 0.5f;
        while(time > 0) {
            time -= Time.deltaTime;
            _llaser.SetPosition(0, transform.position);
            _llaser.SetPosition(1, (FindObjectOfType<PlayerEntity>().transform.position - transform.position) * 2 + transform.position);
            width = Mathf.Lerp(0.5f, 0, Mathf.InverseLerp(_laseraim,0,time));
            if(width < 0.1) {
                width = 0.1f;
            }
            _llaser.startWidth = width;
            _llaser.endWidth = width;
            yield return null;
        }
    }

    IEnumerator Shoot() {
        _llaser.colorGradient = g2;
        _llaser.startWidth = 1;
        _llaser.endWidth = 1;
        _laser.Collider.enabled = true;
        _laser.Collider.size = new Vector2((FindObjectOfType<PlayerEntity>().transform.position - transform.position).magnitude * 2 - 2, 1);
        yield return new WaitForSeconds(_lasershot);
        _laser.Collider.enabled = false;
        _llaser.startWidth = 0;
        _llaser.endWidth = 0;
    }

    void Hit() {
        health--;
        if (health <= 0) {
            Die();
        } else {
            NewSensiblePoint();
            Teleport();
        }
    }

    void Teleport() {
        transform.position = FindObjectOfType<PlayerEntity>().transform.position + new Vector3(Random.value, Random.value, 0).normalized * _teleportDist;
    }

    void Die() {
        Destroy(gameObject);
    }
}
