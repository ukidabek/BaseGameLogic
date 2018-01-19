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
        private Editor characterSettingsEditor;
        private Editor characterEquipmentEditor;
        protected List<ScriptableObject> scriptableObjects = new List<ScriptableObject>();
        private bool[] showEditor = null;
        private Editor[] editors = null;

        protected void AddScriptableObject(ScriptableObject scriptableObject)
        {
            if(scriptableObject != null && !scriptableObjects.Contains(scriptableObject))
                scriptableObjects.Add(scriptableObject);
        }

        protected virtual void Initialize()
        {
            showEditor = new bool[scriptableObjects.Count];
            editors = new Editor[scriptableObjects.Count];

            for (int i = 0; i < scriptableObjects.Count; i++)
            {
                editors[i] = Editor.CreateEditor(scriptableObjects[i]);
            }
        }

        public void OnEnable()
        {
            Initialize();
        }

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            base.OnInspectorGUI();
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(target);
                Initialize();
            }

            for (int i = 0; i < scriptableObjects.Count; i++)
            {
                ScriptableObject scriptableObject = scriptableObjects[i];
                string[] stringsInType = scriptableObject.GetType().ToString().Split('.');
                showEditor[i] = EditorGUILayout.Foldout(showEditor[i], stringsInType[1]);
                if (editors[i] != null && showEditor[i])
                {
                    editors[i].OnInspectorGUI();
                }                
            }
        }
    }
}