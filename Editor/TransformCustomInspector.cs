using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(Transform))]
public class TransformCustomInspector : Editor
{
    private static float LABEL_WIDTH = 52f;
    private static float RESET_BUTTON_WIDTH = 18f;
    private Transform _item = null;

    private void OnEnable()
    {
        _item = target as Transform;    
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        float oldLabelWidth = EditorGUIUtility.labelWidth;

        EditorGUIUtility.labelWidth = LABEL_WIDTH;
        EditorGUILayout.BeginHorizontal();
        {
            if(_item.parent != null)
            {
                _item.localPosition = EditorGUILayout.Vector3Field("Position", _item.localPosition);
            }
            else
            {
                _item.position = EditorGUILayout.Vector3Field("Position", _item.position);
            }

            if (GUILayout.Button("R", GUILayout.Width(RESET_BUTTON_WIDTH)))
            {
                _item.ResetPosition();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUIUtility.labelWidth = LABEL_WIDTH;
        EditorGUILayout.BeginHorizontal();
        {
            _item.rotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Rotation", _item.rotation.eulerAngles));
            if (GUILayout.Button("R", GUILayout.Width(RESET_BUTTON_WIDTH)))
            {
                _item.ResetRotation();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUIUtility.labelWidth = LABEL_WIDTH;
        EditorGUILayout.BeginHorizontal();
        {
            _item.localScale = EditorGUILayout.Vector3Field("Scale", _item.localScale);
            if (GUILayout.Button("R", GUILayout.Width(RESET_BUTTON_WIDTH)))
            {
                _item.ResetScale();
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUIUtility.labelWidth = oldLabelWidth;
    }
}
