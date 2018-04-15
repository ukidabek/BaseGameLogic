using System.Collections.Generic;
using UnityEngine;

public static class TransformExtension
{
    public static void Reset(this Transform transform)
    {
        ResetPosition(transform);
        ResetRotation(transform);
        ResetScale(transform);
    }

    public static void ResetPosition(this Transform transform)
    {
        if(transform.parent == null)
            transform.position = Vector3.zero;
        else
            transform.localPosition = Vector3.zero;
    }

    public static void ResetRotation(this Transform transform)
    {
        if (transform.parent == null)
            transform.rotation = Quaternion.identity;
        else
            transform.rotation = Quaternion.identity;
    }

    public static void ResetScale(this Transform transform)
    {
        transform.localScale = Vector3.one;
    }

    public static float Distance(this Transform transform, Transform _transform)
    {
        return Vector3.Distance(transform.position, _transform.position);
    }

    public static Transform GetRootTransform(this Transform parent)
    {
        if(parent.parent == null)
            return parent;
        else
            return GetRootTransform(parent.parent);
    }

    public static Transform[] GetChildTransforms(this Transform transform)
    {
        List<Transform> transformList = new List<Transform>();

        for (int i = 0; i < transform.childCount; i++)
        {
            transformList.Add(transform.GetChild(i));
        }

        return transformList.ToArray();
    }

    public static GameObject[] GetChildGameObjects(this Transform transform)
    {
        List<GameObject> transformList = new List<GameObject>();

        for (int i = 0; i < transform.childCount; i++)
        {
            transformList.Add(transform.GetChild(i).gameObject);
        }

        return transformList.ToArray();
    }

    public static Vector3 GetWorldPointOnCircle(this Transform transform, float angle, float radius)
    {
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 localPosition = rotation * (Vector3.zero + Vector3.forward * radius);
        return transform.TransformPoint(localPosition);
    }
}
