using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHoldable {
    public EntityHolding Landmaster { get; }

    public void Pickup(EntityHolding entityHolding);
    public void Drop(EntityHolding entityHolding, Vector2 position);
    public void Throw(EntityHolding entityHolding, Vector2 direction, Collider2D thrower);
    public void Throw(EntityHolding entityHolding, Vector2 direction, GameObject thrower);
}
