using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class GameObjectExtension
{
    /// <summary>
    /// Tray get component form parent, object and it's children's.
    /// </summary>
    /// <typeparam name="T">Type of component to find.</typeparam>
    /// <param name="gameObject">Reference to object.</param>
    /// <returns>Reference to component.</returns>
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

    /// <summary>
    /// Create instance of prefab, and return component.
    /// </summary>
    /// <typeparam name="T">Type of component to return.</typeparam>
    /// <param name="prefab">Prefab reference</param>
    /// <param name="parrent">Parent object</param>
    /// <param name="objectName">Object name</param>
    /// <returns>Reference to component.</returns>
    public static T CreateInstance<T>(GameObject prefab, Transform parrent = null, string objectName = "") where T : Component
    {
        GameObject newObject = GameObject.Instantiate(prefab);
        newObject.name = !string.IsNullOrEmpty(objectName) ? objectName : newObject.name;
        T newComponent = newObject.GetComponent<T>();

        newObject.transform.SetParent(parrent);

        newObject.transform.Reset();

        return newComponent;
    }

    /// <summary>
    /// Create new object and add component to it.
    /// </summary>
    /// <typeparam name="T">Type of component to add.</typeparam>
    /// <param name="parent">Parent object.</param>
    /// <param name="objectName">Name of the new object.</param>
    /// <returns>Reference to added component.</returns>
    public static T CreateObjectWithComponent<T>(Transform parent = null, string objectName = "") where T : Component
    {
        GameObject newGameObject = new GameObject();

        if (!string.IsNullOrEmpty(objectName))
        {
            newGameObject.name = objectName;
        }

        newGameObject.transform.SetParent(parent);

        newGameObject.transform.Reset();

        T component = newGameObject.AddComponent<T>();

        return component;
    }
}
