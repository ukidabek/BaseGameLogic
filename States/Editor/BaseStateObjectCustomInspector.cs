using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.LogicModule;

namespace BaseGameLogic.States
{
    [CustomEditor(typeof(BaseStateObject), true)]
    public class BaseStateObjectCustomInspector : Editor
    {
        BaseStateObject stateObject = null;

        protected virtual void OnEnable()
        {
            stateObject = target as BaseStateObject;    
        }

        private void FindAllLogicModule(GameObject gameObject)
        {
            BaseLogicModule[] baseLogicModules = gameObject.GetComponents<BaseLogicModule>();

            for (int i = 0; i < baseLogicModules.Length; i++)
            {
                try
                {
                    stateObject.LogicModulesContainer.AddModule(baseLogicModules[i]);
                }
                catch(LogicModuleOnListException e)
                {
                    Debug.Log(e.Message);
                    DestroyImmediate(e.AdditionalModule);
                }
            }

            for (int i = 0; i < gameObject.transform.childCount; i++)
            {
                FindAllLogicModule(gameObject.transform.GetChild(i).gameObject);
            }
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool guiEnabled = GUI.enabled;
            GUI.enabled = !Application.isPlaying;

            if (GUILayout.Button("Get all logic modules"))
            {
                FindAllLogicModule(stateObject.gameObject);
            }

            GUI.enabled = guiEnabled;
        }
    }
}