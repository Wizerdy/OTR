using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IReflectable {
    void Reflect(ContactPoint2D collision);
    void Launch(float force, Vector2 direction);
}