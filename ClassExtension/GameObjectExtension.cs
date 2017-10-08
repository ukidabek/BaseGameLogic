using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameObjectExtension
{
    public static T DeepGetComponent<T>(this GameObject gameObject) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component != null)
            return component;

        component = gameObject.GetComponentInChildren<T>();
        if (component != null)
            return component;

        component = gameObject.GetComponentInParent<T>();
        return component;
    }
}
