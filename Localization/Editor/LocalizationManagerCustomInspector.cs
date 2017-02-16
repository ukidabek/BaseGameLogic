using UnityEngine;
using UnityEditor;

using System.Collections;

namespace BaseGameLogic.Localization
{
	[CustomEditor(typeof(LocalizationManager))]
	public class LocalizationManagerCustomInspector : Editor 
	{
		private LocalizationManager _localizationManagerInstance = null;

		private void OnEnable()
		{
			_localizationManagerInstance = target as LocalizationManager;
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			EditorGUI.BeginDisabledGroup(!Application.isPlaying);
			{				
				bool openLocalizationManagerTesterWindowButtonPressed = GUILayout.Button ("Open test window");
				if (openLocalizationManagerTesterWindowButtonPressed) 
				{
					LocalizationManagerTesterWindow window = ScriptableObject.CreateInstance<LocalizationManagerTesterWindow> ();
					window.SetLocalizationManagerTesterWindow (_localizationManagerInstance);
				}
			}
			EditorGUI.EndDisabledGroup ();
		}
	}
}
