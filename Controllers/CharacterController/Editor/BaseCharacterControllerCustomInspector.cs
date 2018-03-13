using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Character
{
    [CustomEditor(typeof(BaseCharacterController), true)]
    public class BaseCharacterControllerCustomInspector : Editor 
    {
        public void OnEnable()
        {
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            
            base.OnInspectorGUI();

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
            }

        }
    }
}