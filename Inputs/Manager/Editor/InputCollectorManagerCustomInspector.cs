using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;

namespace BaseGameLogic.Inputs
{
    [CustomEditor(typeof(BaseInputCollectorManager), true)]
    public class InputCollectorManagerCustomInspector : Editor
    {
        [MenuItem("BaseGameLogic/Inputs/Create InputCollectorManager")]
        public static BaseInputCollectorManager CreateInputCollectorManager()
        {
            return GameObjectExtension.CreateInstanceOfAbstractType<BaseInputCollectorManager>();
        }

        private BaseInputCollectorManager inputCollectorManager = null;
        private ReorderableList list = null;

        private void OnEnable() 
        {
            inputCollectorManager = target as BaseInputCollectorManager;
            list = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("_inputCollectors"),
                true, true, true, true);

            list.drawHeaderCallback = DrawHeader;
            list.drawElementCallback = DrawListElement;
            list.onAddCallback = AddElement;
            list.onRemoveCallback = RemoveElement;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();
        }

        #region Reorderable list handling

        private void DrawListElement(Rect rect, int i, bool isActive, bool isFocused)
        {
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(i);
            float playerNumberWidth = 20f;

            Rect elementRect = new Rect(rect.x + 2, rect.y + 2, rect.width - 2 - playerNumberWidth, EditorGUIUtility.singleLineHeight);
            Rect playerNumberRect = new Rect(rect.x + elementRect.width + 2, rect.y + 2, playerNumberWidth, EditorGUIUtility.singleLineHeight);

            EditorGUI.PropertyField(elementRect, element, GUIContent.none);
            EditorGUI.IntField(playerNumberRect, inputCollectorManager[i].PlayerNumber);
        }

        private void AddElement(ReorderableList list)
        {
            inputCollectorManager.AddInputCollector();
        }

        private void RemoveElement(ReorderableList list)
        {
            inputCollectorManager.RemoveAt(list.index);
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Input Collectors");
        }

        #endregion
    }
}