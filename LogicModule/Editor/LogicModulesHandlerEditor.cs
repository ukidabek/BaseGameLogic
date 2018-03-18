using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System;

using System.Collections.Generic;

namespace BaseGameLogic.LogicModule
{
    [CustomEditor(typeof(BaseLogicModulesHandler), true)]
    public class LogicModulesHandlerEditor: Editor
    {
        private BaseLogicModulesHandler _logicModulesHandler = null;
        private ReorderableList list = null;

        private Type[] _logicModulesTypes = null;

        private GenericMenu _addModuleToChildMenu = null;

        protected virtual void OnEnable()
        {
            _logicModulesHandler = target as BaseLogicModulesHandler;
            _logicModulesTypes = AssemblyExtension.GetDerivedTypes<BaseLogicModule>();

            List<GameObject> _gameObjectList = new List<GameObject>();
            _gameObjectList.Add(_logicModulesHandler.gameObject);
            _gameObjectList.AddRange(_logicModulesHandler.transform.GetChildGameObjects());
            _gameObjectList.Add(null);

            _addModuleToChildMenu = GenericMenuExtension.GenerateMenuFromTypesToObject(
                _gameObjectList.ToArray(), 
                _logicModulesTypes, 
                AddModuleToChild);

            list = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("logicModulesContainer").FindPropertyRelative("_modulesList"),
                true, true, true, true);

            list.drawHeaderCallback = DrawHeader;
            list.drawElementCallback = DrawListElement;
            list.onRemoveCallback = RemoveElement;
            list.onAddCallback = AddElement;
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
            if(targetObject == null)
            {
                targetObject = new GameObject();
                targetObject.transform.SetParent(_logicModulesHandler.transform, false);
            } 

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

        private void AddModuleToChild(object obj)
        {
            GameObjectModuleTypePair pair = obj as GameObjectModuleTypePair;
            AddModule(pair.Type, pair.GameObject);
        }

        public override void OnInspectorGUI()
        {
            bool guiEnabled = GUI.enabled;
            GUI.enabled = !Application.isPlaying;

            base.OnInspectorGUI();

            serializedObject.Update();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
            
            GUI.enabled = guiEnabled;
        }

        #region Reorderable list handling

        private void DrawListElement(Rect rect, int i, bool isActive, bool isFocused)
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(i);
            Rect elementRect = new Rect(rect.x + 2, rect.y + 2, rect.width, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(elementRect, element, GUIContent.none);
        }

        private void AddElement(ReorderableList list)
        {
            _addModuleToChildMenu.ShowAsContext();
        }

        private void RemoveElement(ReorderableList list)
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(list.index);
            BaseLogicModule module = element.objectReferenceValue as BaseLogicModule;

            if(module != null)
            {
                DestroyImmediate(module);
                ReorderableList.defaultBehaviours.DoRemoveButton(list);
            }

            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }

        private void DrawHeader(Rect rect)
        {
            bool guiEnabled = GUI.enabled;
            GUI.enabled = !Application.isPlaying;

            if(GUI.Button(rect, "Get all logic modules"))
                FindAllLogicModule(_logicModulesHandler.gameObject);

            GUI.enabled = guiEnabled;
        }

        #endregion
    }
}