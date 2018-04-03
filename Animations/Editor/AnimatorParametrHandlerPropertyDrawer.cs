using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;

namespace BaseGameLogic.Animation
{
	[CustomPropertyDrawer(typeof(AnimatorParametrHandler))]
	public class AnimatorParametrHandlerPropertyDrawer : PropertyDrawer 
	{
		private List<string> parameters = new List<string>(); 
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight * 4;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);
			{
				position.height = EditorGUIUtility.singleLineHeight;
				EditorGUI.LabelField(position, label.text);
				Rect labelPosition, fieldPosition;
				labelPosition = position;
				labelPosition.width /= 2;
				fieldPosition = labelPosition;
				fieldPosition.x += labelPosition.width;

				position.y += EditorGUIUtility.singleLineHeight;
				SerializedProperty animatorProperty = property.FindPropertyRelative("_animator");
				SerializedProperty nameProperty = property.FindPropertyRelative("_name");
				SerializedProperty hashProperty = property.FindPropertyRelative("_nameHash");

				labelPosition.y = fieldPosition.y = position.y;
				position = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
				EditorGUI.LabelField(labelPosition, "Animator");
				EditorGUI.PropertyField(fieldPosition, animatorProperty, GUIContent.none);

				Animator animator = animatorProperty.objectReferenceValue as Animator;
				if(animator != null)
				{
					AnimatorController animatorController =  animator.runtimeAnimatorController as AnimatorController;
					if(animatorController != null)
					{
						if(animatorController.parameters.Length != parameters.Count)
						{
							parameters.Clear();
							foreach (var item in animatorController.parameters)
							{
								parameters.Add(item.name);
							}
						}

						labelPosition.y = fieldPosition.y += EditorGUIUtility.singleLineHeight;

						int index = parameters.IndexOf(nameProperty.stringValue);
						EditorGUI.LabelField(labelPosition, "Parametr name: ");
						index = EditorGUI.Popup(fieldPosition, index, parameters.ToArray());
						if(index > -1)
						{
							nameProperty.stringValue = parameters[index];
							hashProperty.intValue = animatorController.parameters[index].nameHash;
						}
					}
				}
				else
				{
					labelPosition.y = fieldPosition.y += EditorGUIUtility.singleLineHeight;
					EditorGUI.LabelField(labelPosition, "Parametr name: ");
					EditorGUI.PropertyField(fieldPosition, nameProperty, GUIContent.none);
					hashProperty.intValue = Animator.StringToHash(nameProperty.stringValue);
				}
				labelPosition.y = fieldPosition.y += EditorGUIUtility.singleLineHeight;
				EditorGUI.LabelField(labelPosition, "Name hash: ");
				EditorGUI.LabelField(fieldPosition, hashProperty.intValue.ToString());

			}
			EditorGUI.EndProperty();
		}
	}
}