using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace BaseGameLogic.logicElements
{
	[CustomPropertyDrawer(typeof(GenericComparisonLogicElement))]
	public class GenericComparisonLogicElementProperty : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight * 2;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			Rect rect = position;
			rect.width = position.width / 2;
			rect.height = EditorGUIUtility.singleLineHeight;

			EditorGUI.PropertyField(rect, property.FindPropertyRelative("_comparisionType"), GUIContent.none);
			rect.y = position.y + EditorGUIUtility.singleLineHeight;

			SerializedProperty selectedObject = property.FindPropertyRelative("_selectedObject");
			Component component = selectedObject.objectReferenceValue as Component;
            GameObject componentGameObject = component.gameObject;

            List<string> fieldsName = new List<string>();
			Component [] componentsList = componentGameObject.GetComponents<Component>();
            for (int i = 0; i < componentsList.Length; i++)
            {
                Type type = componentsList[i].GetType();
                List<MemberInfo> fields = new List<MemberInfo>();
                fields.AddRange(type.GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
                fields.AddRange(type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance));
                for (int j = 0; j < fields.Count; j++)
                {
                    Type fieldType = null;
                    if(fields[j] is FieldInfo)
                    {
                        fieldType = (fields[j] as FieldInfo).DeclaringType;
                    } else if(fields[j] is PropertyInfo)
                    {
                        fieldType = (fields[j] as PropertyInfo).DeclaringType;
                    }

                    if(fieldType != null && (type == typeof(int) || type == typeof(float)))
                    {
                        fieldsName.Add(fields[j].Name);
                    }
                }
            }

			EditorGUI.PropertyField(rect, selectedObject, GUIContent.none);
			rect.x += rect.width;

			EditorGUI.PropertyField(rect, property.FindPropertyRelative("_value"), GUIContent.none);
		}
	}
}