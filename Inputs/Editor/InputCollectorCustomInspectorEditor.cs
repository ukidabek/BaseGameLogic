using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System;
using System.Collections;

namespace BaseGameLogic.Inputs
{
    [CustomEditor(typeof(BaseInputCollector), true)]
    public class InputCollectorCustomInspectorEditor : Editor 
    {
		protected BaseInputCollector inputCollector = null;

        private ReorderableList list = null;

        private Type[] _inputSourcesTypes = null;
        private GenericMenu _inputSourcesMenu = null;

        private void OnEnable()
        {
            inputCollector = target as BaseInputCollector;

            list = new ReorderableList(
                serializedObject,
                serializedObject.FindProperty("inputSources"),
                true, true, true, true);

            list.drawHeaderCallback = DrawHeader;
            list.drawElementCallback = DrawListElement;
            list.onAddCallback = AddElement;
            list.onRemoveCallback = RemoveElement;
            
            _inputSourcesTypes = AssemblyExtension.GetDerivedTypes<BaseInputSource>();
            _inputSourcesMenu = GenericMenuExtension.GenerateMenuFormTypes(_inputSourcesTypes, AddNewSource);
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            serializedObject.Update();
            list.DoLayoutList();
            serializedObject.ApplyModifiedProperties();

			if (GUILayout.Button("Check inputs assign"))
			{
				inputCollector.CheckInputs ();
			}
        }

        private void AddNewSource(object obj)
        {
            inputCollector.AddInputSource(_inputSourcesTypes[(int)obj]);
        }

        #region Reorderable list handling

        private void DrawListElement(Rect rect, int i, bool isActive, bool isFocused)
        {
            float editButtonWidth = 50f;
            SerializedProperty element = list.serializedProperty.GetArrayElementAtIndex(i);

            Rect elementRect = new Rect(rect.x + 2, rect.y + 2, rect.width - editButtonWidth - 2, EditorGUIUtility.singleLineHeight);
            Rect editButtonRect = new Rect(rect.x + 2 + rect.width - editButtonWidth, rect.y + 2, editButtonWidth, EditorGUIUtility.singleLineHeight);
            
            EditorGUI.PropertyField(elementRect, element, GUIContent.none);
            if(GUI.Button(editButtonRect, "Edit"))
                new InputSourceEditor(inputCollector, i);

        }

        private void AddElement(ReorderableList list) 
        {
            _inputSourcesMenu.ShowAsContext();
        }

        private void RemoveElement(ReorderableList list) 
        {
            inputCollector.RemoveAt(list.index);
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
            ReorderableList.defaultBehaviours.DoRemoveButton(list);
        }

        private void DrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Input sources");
        }

        #endregion
    }
}