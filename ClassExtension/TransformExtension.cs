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
}
