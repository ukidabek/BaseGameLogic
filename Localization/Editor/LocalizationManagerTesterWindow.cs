using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Localization
{
	public class LocalizationManagerTesterWindow : EditorWindow 
	{
		private string _key = "";
		private List<string> _keysList = new List<string>();
		private string _localizationValue = "";
		private List<string> _localizationValuesList = new List<string>();

		private LocalizationManager _localizationManagerInstance = null;

		public void  SetLocalizationManagerTesterWindow (LocalizationManager _LocalizationManagerInstance)
		{
			this._localizationManagerInstance = _LocalizationManagerInstance;
			this.Show ();
		}

		private void OnGUI ()
		{
			bool isPlaying = Application.isPlaying;
			if (isPlaying && _localizationManagerInstance != null) 
			{
				string[] localizationsNames = _localizationManagerInstance.AvailableLocales;
				EditorGUILayout.BeginHorizontal ();
				{
					for (int i = 0; i < localizationsNames.Length; i++) 
					{
						bool localizationSelectButtonPressed = GUILayout.Button (localizationsNames [i]);
						if (localizationSelectButtonPressed) 
						{
							_localizationManagerInstance.SelectLocalization (i);
							_localizationValue = _localizationManagerInstance.GetTranslation (_key);
						
							_localizationValuesList.Clear ();
							for (int j = 0; j < _keysList.Count; j++) 
							{
								string value = _localizationManagerInstance.GetTranslation (_keysList [j]);
								_localizationValuesList.Add(value);
							}
						}
					}
				}
				EditorGUILayout.EndHorizontal();

				EditorGUILayout.BeginHorizontal ();
				{
					_key = EditorGUILayout.TextField ("Localization Key: ", _key);
					bool getLocalizationValueButtonPressed = GUILayout.Button ("Get localization value.");
					if (getLocalizationValueButtonPressed) 
					{
						if (!_keysList.Contains (_key)) 
						{
							_keysList.Add (_key);
						}

						_localizationValue = _localizationManagerInstance.GetTranslation (_key);

						_localizationValuesList.Clear ();
						for (int i = 0; i < _keysList.Count; i++) 
						{
							string value = _localizationManagerInstance.GetTranslation (_keysList [i]);
							_localizationValuesList.Add(value);
						}
					}
				}
				EditorGUILayout.EndHorizontal();
			
				EditorGUILayout.LabelField (_localizationValue);

				for (int i = 0; i < _localizationValuesList.Count; i++) 
				{
					string value =_localizationValuesList[i];
					EditorGUILayout.LabelField (value);
				}
			}
		}
	}
}