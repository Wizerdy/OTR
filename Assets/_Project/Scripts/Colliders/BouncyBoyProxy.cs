using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncyBoyProxy : MonoBehaviour, IReflectable {
    [SerializeField] BouncyBoy _bouncyBoy;

    public void Launch(float force, Vector2 direction) {
        _bouncyBoy.Launch(force, direction);
    }

    public void Reflect(ContactPoint2D collision) {
        _bouncyBoy.Reflect(collision);
    }
}
