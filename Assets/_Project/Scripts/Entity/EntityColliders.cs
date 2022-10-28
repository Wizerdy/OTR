using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityColliders : MonoBehaviour, IEntityAbility {
    [SerializeField] ColliderDelegate _mainCollider;

    public ColliderDelegate Main => _mainCollider;
}
