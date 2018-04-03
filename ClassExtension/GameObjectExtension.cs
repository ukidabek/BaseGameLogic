using UnityEngine;

using System;
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
    public static T GetComponentDeep<T>(this GameObject gameObject, bool includeInactive = false) where T : Component
    {
        T component = gameObject.GetComponent<T>();
        if (component != null)
            return component;

        component = gameObject.GetComponentInChildren<T>(includeInactive);
        if (component != null)
            return component;

        component = gameObject.GetComponentInParent<T>();
        return component;
    }

    public static GameObject CreateInstance(this GameObject gameObject, Transform parrent = null, string objectName = "")
    {
        GameObject newObject = gameObject.CreateInstance(gameObject, parrent, objectName);
        return newObject;
    }

    /// <summary>
    /// Create instance of prefab, and return gameObject.
    /// </summary>
    /// <param name="prefab">Prefab reference</param>
    /// <param name="parrent">Parent object transform</param>
    /// <param name="objectName">Object name</param>
    /// <returns>Created object</returns>
    public static GameObject CreateInstance(this GameObject gameObject, GameObject prefab, Transform parrent = null, string objectName = "")
    {
        GameObject newObject = GameObject.Instantiate(prefab);

        newObject.transform.SetParent(parrent);
        newObject.transform.Reset();
        newObject.name = !string.IsNullOrEmpty(objectName) ? objectName : newObject.name;

        return newObject;
    }

    /// <summary>
    /// Create instance of prefab, and return component.
    /// </summary>
    /// <typeparam name="T">Type of component to return.</typeparam>
    /// <param name="prefab">Prefab reference</param>
    /// <param name="parrent">Parent object transform</param>
    /// <param name="objectName">Object name</param>
    /// <returns>Reference to component.</returns>
    public static T CreateInstance<T>(this GameObject gameObject, GameObject prefab, Transform parrent = null, string objectName = "") where T : Component
    {
        GameObject newObject = gameObject.CreateInstance(prefab, parrent, objectName);

        T newComponent = newObject.GetComponent<T>();

        return newComponent;
    }

    /// <summary>
    /// Create new object and add component to it.
    /// </summary>
    /// <typeparam name="T">Type of component to add.</typeparam>
    /// <param name="parent">Parent object.</param>
    /// <param name="objectName">Name of the new object.</param>
    /// <returns>Reference to added component.</returns>
    public static T CreateObjectWithComponent<T>(this GameObject gameObject, Transform parent = null, string objectName = "") where T : Component
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

    #if UNITY_EDITOR

    /// <summary>
    /// Create object and add to it component of type "T", and return that instance. 
    /// </summary>
    /// <returns>Reference to component.</returns>
    public static T CreateInstanceOfAbstractType<T>() where T: MonoBehaviour
    {
        Type providerType = typeof(T);
        if(!providerType.IsAbstract)
        {
            Debug.LogErrorFormat("Provided type {0} is not abstract.", providerType.Name);
            return null;
        }

        Type[] types = AssemblyExtension.GetDerivedTypes<T>();
        if (types != null && types.Length > 0)
        {
            GameObject newGameObject = new GameObject();
            newGameObject.name = types[0].Name;
            return newGameObject.AddComponent(types[0]) as T;
        }
        else
        {
            Debug.LogErrorFormat("There is no class that extends abstract class {0}.", typeof(T).Name);
        }

        return null;
    }
    #endif
}
