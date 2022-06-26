using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldable {
    public void Pickup(EntityHolding entityHolding);
    public void Drop(Vector2 position);
    public void Throw(Vector2 direction, Collider2D thrower);
    public void Throw(Vector2 direction, GameObject thrower);
}
