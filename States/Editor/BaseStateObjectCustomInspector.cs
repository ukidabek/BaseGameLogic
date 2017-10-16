using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;
using BaseGameLogic.Inputs;
using BaseGameLogic.Audio;
using BaseGameLogic.Events;

namespace BaseGameLogic.States
{
    /// <summary>
    /// Base game object.
    /// </summary>
    [CustomEditor(typeof(BaseStateObject))]
    public class BaseStateObjectCustomInspector : Editor
    {
        private BaseStateObject _stateObject = null;

        private bool _showStates = false;

        protected string[] stringsInType;
        protected Editor[] stateEditors;
        protected bool [] showStateEditors;

        protected virtual void OnEnable()
        {
            _stateObject = target as BaseStateObject;

            stringsInType = new string[_stateObject.StateCreators.Count + 1];
            stateEditors = new Editor[_stateObject.StateCreators.Count + 1];
            showStateEditors = new bool[_stateObject.StateCreators.Count + 1];

            stateEditors[0] = CreateEditor(_stateObject.DefaultStateCreator);
            string[] typeString = _stateObject.DefaultStateCreator.GetType().ToString().Split('.');
            stringsInType[0] = typeString[typeString.Length - 1];

            for (int i = 0; i < _stateObject.StateCreators.Count; i++)
            {
                typeString = _stateObject.StateCreators[i].GetType().ToString().Split('.');
                stringsInType[i + 1] = typeString[typeString.Length - 1];
                stateEditors[i +1] = CreateEditor(_stateObject.StateCreators[i]);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical();
            {
                
                _showStates = EditorGUILayout.Foldout(_showStates, "States");
                if(_showStates)
                {
                    for (int i = 0; i < stateEditors.Length; i++)
                    {
                        showStateEditors[i] = EditorGUILayout.Foldout(showStateEditors[i], stringsInType[i]);
                        if (stateEditors[i] != null && showStateEditors[i])
                        {
                            stateEditors[i].OnInspectorGUI();
                        }
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }
    }
}