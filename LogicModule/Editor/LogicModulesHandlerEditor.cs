using UnityEngine;
using UnityEditor;

using System;

using System.Collections.Generic;

namespace BaseGameLogic.LogicModule
{
    [CustomEditor(typeof(LogicModulesHandler))]
    public class LogicModulesHandlerEditor: Editor
    {
        private LogicModulesHandler _logicModulesHandler = null;
        private Type[] _logicModulesTypes = null;

        private GenericMenu _addModuleMenu = null;
        private GenericMenu _addModuleToChildMenu = new GenericMenu();

        private List<Transform> _childTransformList = new List<Transform>();
        
        protected virtual void OnEnable()
        {
            _logicModulesHandler = target as LogicModulesHandler;
            _logicModulesTypes = AssemblyExtension.GetDerivedTypes<BaseLogicModule>();

            _addModuleMenu = GenericMenuExtension.GenerateMenuFormTypes(_logicModulesTypes, AddModule);

            GUIContent content = null;
            for (int i = 0; i < _logicModulesHandler.transform.childCount; i++)
            {
                GameObject gameObject = _logicModulesHandler.transform.GetChild(i).gameObject;
                for (int j = 0; j < _logicModulesTypes.Length; j++)
                {
                    content = new GUIContent(string.Format("{0}/{1}", gameObject.name, _logicModulesTypes[j].Name));
                    GameObjectModuleTypePair pair = new GameObjectModuleTypePair(gameObject, _logicModulesTypes[j]);
                    _addModuleToChildMenu.AddItem(content, false, ShowAddMenuForChild, pair);
                }
            }
        }

        private void FindAllLogicModule(GameObject gameObject)
        {
            BaseLogicModule[] baseLogicModules = gameObject.GetComponents<BaseLogicModule>();

            for (int i = 0; i < baseLogicModules.Length; i++)
            {
                try
                {
                    _logicModulesHandler.LogicModulesContainer.AddModule(baseLogicModules[i]);
                }
                catch (LogicModuleOnListException e)
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

        private void AddModule(object obj)
        {
            AddModule(_logicModulesTypes[(int)obj], _logicModulesHandler.gameObject);
        }

        private void AddModule(Type moduleType, GameObject targetObject)
        {
            BaseLogicModule module = targetObject.AddComponent(moduleType) as BaseLogicModule;
            try
            {
                _logicModulesHandler.AddModule(module);
            }
            catch(LogicModuleOnListException exception)
            {
                Debug.LogException(exception);
                DestroyImmediate(module);
            }
        }

        private void ShowAddMenuForChild(object obj)
        {
            GameObjectModuleTypePair pair = obj as GameObjectModuleTypePair;
            AddModule(pair.Type, pair.GameObject);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool guiEnabled = GUI.enabled;
            GUI.enabled = !Application.isPlaying;

            if (GUILayout.Button("Get all logic modules"))
            {
                FindAllLogicModule(_logicModulesHandler.gameObject);
            }

            EditorGUILayout.BeginHorizontal();
            {
                if(GUILayout.Button("Add logic module"))
                {
                    _addModuleMenu.ShowAsContext();
                }

                if (GUILayout.Button("Add logic to child"))
                {
                    _addModuleToChildMenu.ShowAsContext();
                }

            }
            EditorGUILayout.EndHorizontal();

            GUI.enabled = guiEnabled;
        }
    }

    internal class GameObjectModuleTypePair
    {
        public GameObject GameObject = null;
        public Type Type = null;

        public GameObjectModuleTypePair(GameObject gameObject, Type type)
        {
            GameObject = gameObject;
            Type = type;
        }
    }

}