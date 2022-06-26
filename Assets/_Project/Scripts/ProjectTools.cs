using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectTools {
    public static T GetComponentInRoot<T>(this GameObject obj) where T : class {
        T component = obj.GetComponent<T>();
        if (component != null) {
            return component;
        }
        component = obj.GetComponent<ColliderRoot>()?.Root.GetComponent<T>() ?? null;
        if (component != null) {
            return component;
        }
        return null;
    }

    public static T GetComponentInRoot<T>(this GameObject obj, out GameObject root) where T : class {
        T component = obj.GetComponent<T>();
        if (component != null) {
            root = obj;
            return component;
        }
        root = obj.GetComponent<ColliderRoot>()?.Root;
        component = root?.GetComponent<T>() ?? null;
        if (component != null) {
            return component;
        }
        return null;
    }
}
