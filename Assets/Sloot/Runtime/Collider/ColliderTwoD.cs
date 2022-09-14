using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class ColliderTwoD : MonoBehaviour {

    List<Collision2D> _contacts = new List<Collision2D>();
    public List<Collision2D> GetContacts { get { return _contacts; } }

    [SerializeField] UnityEvent<Collision2D> _onCollision;
    [SerializeField] UnityEvent<Collision2D> _onCollisionEnd;

    public event UnityAction<Collision2D> OnCollision { add => _onCollision.AddListener(value); remove => _onCollision.RemoveListener(value); }
    public event UnityAction<Collision2D> OnCollisionEnd { add => _onCollisionEnd.AddListener(value); remove => _onCollisionEnd.RemoveListener(value); }

    public bool Contact { get { return _contacts.Count == 0 ? false : true; } }


    private void OnCollisionEnter2D(Collision2D collision) {
        _contacts.Add(collision);
        _onCollision?.Invoke(collision);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        _contacts.Remove(collision);
        _onCollisionEnd?.Invoke(collision);

    }
}
