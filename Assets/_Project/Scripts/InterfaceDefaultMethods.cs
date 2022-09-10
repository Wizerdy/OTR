using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InterfaceDefaultMethods {
    public static Vector2 Reflect(Vector2 speed, ContactPoint2D contact) {
        return Vector2.Reflect(speed, contact.normal);
    }
}
