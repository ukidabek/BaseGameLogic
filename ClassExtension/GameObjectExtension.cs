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

    public static T CreateInstance<T>(GameObject prefab, Transform parrent = null, string objectName = "") where T : Component
    {
        GameObject newObject = GameObject.Instantiate(prefab);
        newObject.name = !string.IsNullOrEmpty(objectName) ? objectName : newObject.name;
        T newComponent = newObject.GetComponent<T>();

        newObject.transform.SetParent(parrent);

        newObject.transform.localPosition = Vector3.zero;
        newObject.transform.localRotation = Quaternion.identity;
        newObject.transform.localScale = Vector3.one;

        return newComponent;
    }

    public static T CreateObjectWithComponent<T>(Transform parrent = null, string objectName = "") where T : Component
    {
        GameObject newGameObject = new GameObject();

        if (!string.IsNullOrEmpty(objectName))
        {
            newGameObject.name = objectName;
        }

        newGameObject.transform.SetParent(parrent);

        newGameObject.transform.localPosition = Vector3.zero;
        newGameObject.transform.localRotation = Quaternion.identity;
        newGameObject.transform.localScale = Vector3.one;

        T component = newGameObject.AddComponent<T>();

        return component;
    }
}
