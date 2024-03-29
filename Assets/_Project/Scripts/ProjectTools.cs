using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ProjectTools {
    /// <summary>
    /// Get a component in target or in the ColliderRoot Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
    public static T GetComponentInRoot<T>(this GameObject obj, out T component) where T : class {
        component = obj.GetComponent<T>();
        if (component != null) {
            return component;
        }
        component = obj.GetComponent<ColliderRoot>()?.Root.GetComponent<T>() ?? null;
        if (component != null) {
            return component;
        }
        return null;
    }

    /// <summary>
    /// Get a component in target or in the ColliderRoot Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Get a component in target or in the ColliderRoot Component
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="obj"></param>
    /// <returns></returns>
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

    public static GameObject GetRoot(this GameObject obj) {
        GameObject root;
        root = obj.GetComponent<ColliderRoot>()?.Root ?? obj;
        return root;
    }
}
