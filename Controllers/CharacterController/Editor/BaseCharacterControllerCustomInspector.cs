using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.States;

namespace BaseGameLogic.Character
{
    [CustomEditor(typeof(BaseCharacterController), true)]
    public class BaseCharacterControllerCustomInspector : BaseStateObjectCustomInspector
    {
        private BaseCharacterController baseCharacterController = null;
        private Editor characterSettingsEditor;
        private Editor characterEquipmentEditor;
        protected List<ScriptableObject> scriptableObjects = new List<ScriptableObject>();
        private bool[] showEditors = null;
        private Editor[] editors = null;

        protected void AddScriptableObject(ScriptableObject scriptableObject)
        {
            if(scriptableObject != null && !scriptableObjects.Contains(scriptableObject))
                scriptableObjects.Add(scriptableObject);
        }

        protected virtual void Initialize()
        {
            baseCharacterController = target as BaseCharacterController;
            ScriptableObject tmp = null; 

            tmp = baseCharacterController.Settings as ScriptableObject;
            AddScriptableObject(tmp);

            showEditors = new bool[scriptableObjects.Count];
            editors = new Editor[scriptableObjects.Count];

            for (int i = 0; i < scriptableObjects.Count; i++)
            {
                editors[i] = Editor.CreateEditor(scriptableObjects[i]);
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            Initialize();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            {
                base.OnInspectorGUI();
            }
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
                Initialize();
            }

            for (int i = 0; i < scriptableObjects.Count; i++)
            {
                ScriptableObject scriptableObject = scriptableObjects[i];
                string[] stringsInType = scriptableObject.GetType().ToString().Split('.');
                showEditors[i] = EditorGUILayout.Foldout(showEditors[i], stringsInType[stringsInType.Length - 1]);

                if (editors[i] != null && showEditors[i])
                {
                    editors[i].OnInspectorGUI();
                }                
            }
        }
    }
}