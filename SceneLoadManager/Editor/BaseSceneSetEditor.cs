using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

namespace BaseGameLogic.SceneManagement
{
	[CustomEditor(typeof(BaseSceneSet), true)]
	public class BaseSceneSetEditor : Editor 
	{
        private BaseSceneSet sceneSet = null;
		private ReorderableList _list = null;
		private SceneAsset scene = null;

		private void OnEnable() 
		{
            sceneSet = target as BaseSceneSet;
			_list = new ReorderableList(serializedObject, serializedObject.FindProperty("_sceneInfoList"),
			true,true,true,true);
			_list.drawElementCallback = DrawElement;
			_list.onAddCallback = AddElement;
			_list.drawHeaderCallback = DrawHeader;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			_list.DoLayoutList();
			serializedObject.ApplyModifiedProperties();

			for(int i = 0; i < sceneSet.Count; i++)
			{
				if(SceneInNullOrNotSetAtAll(sceneSet[i]))
					EditorGUILayout.HelpBox("Some scenes are not setup correctly!", MessageType.Error);
			}
		}

		private bool SceneInNullOrNotSetAtAll(SceneInfo info)
		{
			return info == null || (string.IsNullOrEmpty(info.SceneName) && string.IsNullOrEmpty(info.ScenePath));
		}

		private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
		{
			float buttonWidth = 30;
			rect = new Rect(rect.x, rect.y + 2, rect.width - buttonWidth, EditorGUIUtility.singleLineHeight);
			if(SceneInNullOrNotSetAtAll(sceneSet[index]))
			{
				scene = EditorGUI.ObjectField(rect,	scene, typeof(SceneAsset), false) as SceneAsset;
				rect = new Rect(rect.x + rect.width, rect.y, buttonWidth, EditorGUIUtility.singleLineHeight);
				if(GUI.Button(rect, "Set") && scene != null)
				{
					sceneSet[index] = new SceneInfo()
					{
						SceneName = scene.name,
						ScenePath = AssetDatabase.GetAssetPath(scene)
					};
					scene = null;
				}
			}
			else
			{
				EditorGUI.LabelField(rect, sceneSet[index].SceneName);
			}
        }
        
        private void AddElement(ReorderableList list)
        {
			sceneSet.Add(null);
        }

		private void DrawHeader(Rect rect)
		{
			EditorGUI.LabelField(rect, "Scenes list:");
		}
	}
}