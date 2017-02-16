using UnityEngine;
using UnityEditor;

using System.Collections;

namespace BaseGameLogic.Inputs
{
    [CustomEditor(typeof(InputCollector), true)]
    public class InputCollectorCustomInspectorEditor : Editor 
    {
		protected InputCollector inputCollector = null;
		protected InputSourceEnum inputSourceEnum = InputSourceEnum.KeyboardAndMouse;

        private void OnEnable()
        {
            inputCollector = target as InputCollector;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            for (int i = 0; i < inputCollector.InputSources.Count; i++)
            {
                if (inputCollector.InputSources[i] == null)
                {
                    inputCollector.InputSources.RemoveAt(i);
                    --i;
                    continue;
                }

                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.LabelField(inputCollector.InputSources[i].GetType().ToString());
                    if (GUILayout.Button("Edit"))
                    {
                        new InputSourceEditor(inputCollector, i);
                    }

                    if (GUILayout.Button("Remove"))
                    {
                        BaseInputSource source = inputCollector.InputSources[i];
                        inputCollector.InputSources.Remove(source);
                        GameObject.DestroyImmediate(source, true);
                    }
                }
                EditorGUILayout.EndHorizontal();
            }


//            EditorGUILayout.BeginHorizontal();
//            {
//                inputSourceEnum = (InputSourceEnum)EditorGUILayout.EnumPopup(inputSourceEnum);
//                if (GUILayout.Button("Add input source."))
//                {
//                    CreateInputSource(inputSourceEnum);
//                }
//            }
//            EditorGUILayout.EndHorizontal();

			if (GUILayout.Button("Check imputs assign"))
			{
				inputCollector.CheckInpunts ();
			}
        }
			

		protected virtual void CreateInputSource(InputSourceEnum inputSourceEnum) 
        {
            if (inputCollector == null)
            {
                GameObject inputSourcesContainerObject = new GameObject();
                inputCollector.inputSourcesContainerObject = inputSourcesContainerObject;
                inputSourcesContainerObject.transform.SetParent(inputCollector.transform);
                inputSourcesContainerObject.transform.localPosition = Vector3.zero;
            }

            switch (inputSourceEnum)
            {
                case InputSourceEnum.KeyboardAndMouse:
                    
                    break;
            }
        }
    }
}