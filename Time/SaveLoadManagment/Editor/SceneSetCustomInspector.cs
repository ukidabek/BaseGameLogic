using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.SceneManagement
{
    [CustomEditor(typeof(SceneSet), true)]
    public class SceneSetCustomInspector : Editor
    {
        private SceneSet _item = null;
        private Object scene = null;

        private void OnEnable()
        {
            _item = target as SceneSet;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.BeginHorizontal();
            {
                scene = (Object)EditorGUILayout.ObjectField(scene, typeof(Object), false);
                if (scene != null && GUILayout.Button("Add"))
                {
                    string scenePath = AssetDatabase.GetAssetPath(scene);
                    string sceneName = scene.name;

                    SceneInfo sceneInfo = new SceneInfo()
                    {
                        SceneName = sceneName,
                        ScenePath = scenePath
                    };

                    _item.SceneInfoList.Add(sceneInfo);

                    EditorUtility.SetDirty(_item);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}