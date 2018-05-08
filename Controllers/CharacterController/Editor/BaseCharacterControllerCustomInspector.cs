using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.States;

namespace BaseGameLogic.Character
{
    [CustomEditor(typeof(BaseCharacterController), true)]
    public class BaseCharacterControllerCustomInspector : Editor 
    {
        //protected override void OnEnable()
        //{
        //    base.OnEnable();
        //}

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