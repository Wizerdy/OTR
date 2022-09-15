using System.Collections;
using System.Collections.Generic;
using ToolsBoxEngine;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CircleCollider2D))]
public class SensiblePoint : MonoBehaviour {
    public bool SensibleState;
    SpriteRenderer sr;
    [SerializeField] BetterEvent _touche = new BetterEvent();

    public event UnityAction Touche { add => _touche.AddListener(value); remove => _touche.RemoveListener(value); }
    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        UpdateState();
    }
    public void UpdateState() {
        if (SensibleState) {
            sr.color = Color.red;
        } else {
            sr.color = Color.blue;
        }
    }

    void OnTriggerEnter2D(Collider2D collision) {
        if (SensibleState && collision.transform.tag != "Laser") {
            _touche.Invoke();
        }
    }


}
