using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ToolsBoxEngine;
using Sloot;
using System.Linq;

public class Blast : MonoBehaviour {
    [SerializeField] List<IHealth> hits = new List<IHealth>();
    [SerializeField] Material _charging;
    [SerializeField] Material _blasting;
    Vector2[] _points;
    int _damages;
    float _chargeDuration;
    float _blastDuration;
    float _damagesMultiplier;
    PolygonCollider2D _polygonCollider;
    Mesh _mesh;
    MeshFilter _meshFilter;
    public Blast ChangeDamages(int damages) {
        _damages = damages;
        return this;
    }

    public Blast ChangeBlastDuration(float blastDuration) {
        _blastDuration = blastDuration;
        return this;
    } 
    public Blast ChangeChargeDuration(float chargeDuration) {
        _chargeDuration = chargeDuration;
        return this;
    }

    public Blast ChangeDamagesMultipler(float damagesMultiplier) {
        _damagesMultiplier = damagesMultiplier;
        return this;
    }
    private void Start() {
        MeshGeneration();
        ShowBlast();
        StartCoroutine(Tools.Delay(() => BlastArea(), _chargeDuration));
    }

    public void ChangePoints(Vector2[] points) {
        _points = points;
    }

    public void Hit(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            hits.Add(collider.gameObject.GetRoot().GetComponent<IHealth>());
        }
    }

    void ApplyDamages() {
        int damages = _damages;
        if (hits.Count > 1)
            damages = (int)(damages * AlgebraSloot.Pow(_damagesMultiplier, hits.Count - 1));
        for (int i = 0; i < hits.Count; i++) {
            hits[i].TakeDamage(damages, gameObject);
        }
        Die();
    }

    void MeshGeneration() {
        _meshFilter = GetComponent<MeshFilter>();
        _mesh = new Mesh();
        _mesh.vertices = _points.Select((v2) => (Vector3)v2).ToArray();
        int[] triangles = new int[] {
            0, 2, 1,
            0, 5, 2,
            2, 5, 4,
            2, 4, 3
        }; 
        _mesh.triangles = triangles;
        _meshFilter.mesh = _mesh;
    }

    void Die(){
        gameObject.SetActive(false);
        Destroy(gameObject);
    }

    void ShowBlast() {
        gameObject.GetComponent<Renderer>().material = _charging;
    }

    void BlastArea() {
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _polygonCollider.points = _points;
        gameObject.GetComponent<Renderer>().material = _blasting;
        StartCoroutine(Tools.Delay(() => ApplyDamages(), _blastDuration));
    }
}
