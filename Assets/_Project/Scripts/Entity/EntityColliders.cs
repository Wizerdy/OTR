using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityColliders : MonoBehaviour, IEntityAbility {
    [SerializeField] ColliderDelegate _mainCollider;
    [SerializeField] Collider2D _collider;

    public ColliderDelegate MainEvent => _mainCollider;
    public Collider2D Main => _collider;
}
