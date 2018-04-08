using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.States
{
    [CustomEditor(typeof(BaseStateGraph), true)]
    public class BaseStateGraphEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            if(GUILayout.Button("Open editor"))
            {
                EditorWindow window = new StateGraphEditorWindow(target as BaseStateGraph);
            }
        }
    }

}