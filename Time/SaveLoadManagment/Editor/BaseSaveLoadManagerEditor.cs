using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

using System;

namespace BaseGameLogic.SceneManagement
{
    [CustomEditor(typeof(BaseSceneLoadManager), true)]
    public class BaseSaveLoadManagerEditor : Editor
    {
        [MenuItem("BaseGameLogic/SaveLoad/Create SaveLoadManager")]
        public static BaseSceneLoadManager CreateSaveLoadManager()
		{
            return GameObjectExtension.CreateInstanceOfAbstractType<BaseSceneLoadManager>();
        }

        private BaseSceneLoadManager _manager = null;
        
        private ReorderableList list;

        private void OnEnable() 
        {
            _manager = target as BaseSceneLoadManager;
            list = new ReorderableList(serializedObject, serializedObject.FindProperty("_sceneSetList"), true, true, true, true);
            list.drawElementCallback = DrawElement;
            list.drawHeaderCallback = DrawHeader;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            HandleReorderableList();
        }
        
        private void HandleReorderableList()
        {
            serializedObject.Update();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), "Scene Sets List");
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            rect.height = EditorGUIUtility.singleLineHeight;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, 20, EditorGUIUtility.singleLineHeight), index.ToString());
            EditorGUI.PropertyField(new Rect(rect.x + 20, rect.y, rect.width - 20, EditorGUIUtility.singleLineHeight), list.serializedProperty.GetArrayElementAtIndex(index), GUIContent.none);
        }
    }
}