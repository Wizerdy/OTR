using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sloot;
using System.Linq;

public class Blast : MonoBehaviour {
    [SerializeField] List<IHealth> hits = new List<IHealth>();
    Vector2[] _points;
    int _damages;
    float _damagesMultiplier;
    PolygonCollider2D _polygonCollider;
    Mesh _mesh;
    MeshFilter _meshFilter;
    MeshRenderer _meshRenderer;
    public Blast ChangeDamages(int damages) {
        _damages = damages;
        return this;
    }

    public Blast ChangeDamagesMultipler(float damagesMultiplier) {
        _damagesMultiplier = damagesMultiplier;
        return this;
    }
    private void Start() {
        _polygonCollider = GetComponent<PolygonCollider2D>();
        _polygonCollider.points = _points;
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();
        MeshGeneration();
        _meshFilter.mesh = _mesh;
    }

    private void Update() {
        if (hits.Count > 0) {
            ApplyDamages();
        }
    }
    public void ChangePoints(Vector2[] points) {
        _points = points;
    }

    public void Hit(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            Debug.Log(collider.name);
            hits.Add(collider.gameObject.GetRoot().GetComponent<IHealth>());
        }
    }

    void ApplyDamages() {
        int damages = _damages;
        if (hits.Count > 1)
            damages = (int)(damages * AlgebraSloot.Pow(_damagesMultiplier, hits.Count - 1));
        for (int i = 0; i < hits.Count; i++) {
            hits[i].TakeDamage(damages, this.gameObject);
        }
        gameObject.SetActive(false);
    }

    void MeshGeneration() {
        _mesh = new Mesh();
        _mesh.vertices = _points.Select((v2) => (Vector3)v2).ToArray();
        int[] triangles = new int[] {
            0, 2, 1,
            0, 5, 2,
            2, 5, 4,
            2, 4, 3
        };
        _mesh.triangles = triangles;


    }

}
