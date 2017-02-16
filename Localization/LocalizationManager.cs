using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;


namespace BaseGameLogic.Localization
{
	public class LocalizationManager: MonoBehaviour
	{
	    #region MEMBERS

		#region Const values

		private const string Choosen_Localization_PlayerPrefs_Key = "ChoosenLocalization";

		#endregion

		#region Parsing config.
		/// <summary>
		/// The name of the localization resource file.
		/// </summary>
		[SerializeField]
		private List<string> localizationResourceFileNamesList = new List<string>();

		/// <summary>
		/// The new line separator.
		/// </summary>
//		[SerializeField]
		private string _newLineSeparator= "\r\n";

		/// <summary>
		/// The value separator.
		/// </summary>
		[SerializeField]
		private string _valueSeparator = ";";

		#endregion

	    #endregion

	    #region PROPERTIES

		[SerializeField]
		private string _choosenLocalization = "";

		/// <summary>
		/// The available locales <=> language.
		/// </summary>
		[SerializeField]
		private string[] _availableLocales;
		public string[] AvailableLocales {
			get { return this._availableLocales; }
		}

		/// <summary>
		/// The localizations dictionery.
		/// language - translations key - translation
		/// </summary>
		private Dictionary<string, Dictionary<string, string>> _localizationsDictionery = new Dictionary<string, Dictionary<string, string>>();

	    #endregion

	    #region METHODS

		public LocalizationManager () { }

		public LocalizationManager(List<string> localizationResourceFileNamesList, string newLineSeparator, string valueSeparator)
		{
			this.localizationResourceFileNamesList = localizationResourceFileNamesList;
			_newLineSeparator = newLineSeparator;
			_valueSeparator = valueSeparator;
		}


		private void Awake()
		{
			SetDefaultLocalization ();
			LoadLocales ();
		}

		/// <summary>
		/// Sets the default localization.
		/// </summary>
		public void SetDefaultLocalization()
		{
			string choosenLocalizationKey = Choosen_Localization_PlayerPrefs_Key;
			this._choosenLocalization = PlayerPrefs.GetString (choosenLocalizationKey);
		}

		/// <summary>
		/// Selects the localization.
		/// </summary>
		/// <param name="localizationIndex">Localization index.</param>
		public void SelectLocalization(int localizationIndex)
		{
			if (localizationIndex < 0 || localizationIndex > _availableLocales.Length - 1) 
			{
				// Throw exeption
				return;
			}

			string localizationName = _availableLocales [localizationIndex];
			string choosenLocalizationKey = Choosen_Localization_PlayerPrefs_Key;
			PlayerPrefs.SetString (choosenLocalizationKey, localizationName);

			this._choosenLocalization = localizationName; 
		}

		/// <summary>
		/// Return the translated text depending on the selected location.
		/// </summary>
		/// <returns>The translation.</returns>
		/// <param name="key">Key.</param>
	    public string GetTranslation(string key)
	    {
			Dictionary<string, string> localizationDictionary = null;
			bool containsValue = false;
			string localizationValue = string.Empty;

			containsValue = _localizationsDictionery.TryGetValue (_choosenLocalization, out localizationDictionary);
			if (containsValue) 
			{
				containsValue = localizationDictionary.TryGetValue (key, out localizationValue);
				if (containsValue) 
				{
					return localizationValue;
				}
			}

			return string.Empty;
	    }

		private List<string> LoadFiles()
		{
			//	Loading localization file form resources.
			List<string> localizationResourceFileNamesListFiltered = new List<string>();
			int localizationFileCount = localizationResourceFileNamesList.Count;
			for (int i = 0; i < localizationFileCount; i++) 
			{
				string value = localizationResourceFileNamesList [i];
				if (!localizationResourceFileNamesListFiltered.Contains (value))
				{
					localizationResourceFileNamesListFiltered.Add (value);
				}
				else 
				{
					Debug.LogWarningFormat ("You try to use a file {0} twice.", value);
				}
			}

			List<string> localizationFilesContents = new List<string> ();
			localizationFileCount = localizationResourceFileNamesListFiltered.Count;
			for (int i = 0; i < localizationFileCount; i++) 
			{
				TextAsset localizationFile = Resources.Load(localizationResourceFileNamesListFiltered[i]) as TextAsset;
				localizationFilesContents.Add (localizationFile.text);
			}

			return localizationFilesContents;
		}
		private string [] ExtractLines(string localizationFileContents, string[] valueSeparatorsTab)
		{
			//	Parsing lines.
			string [] newLineSeparatorTab = { _newLineSeparator  };
			string [] lines = localizationFileContents.Split(newLineSeparatorTab, StringSplitOptions.RemoveEmptyEntries);
			ExtractLanguages (lines [0], valueSeparatorsTab);

			return lines;
		}

		private void ExtractLanguages(string line, string[] valueSeparatorsTab)
		{
			// Parsing laungiages. 
			string [] knownLangs = line.Split(valueSeparatorsTab, StringSplitOptions.None);

			this._availableLocales = new string[knownLangs.Length - 1];
			for(int i = 1; i < knownLangs.Length; i++)
			{
				this._availableLocales[i - 1] = knownLangs[i];
				if(!this._localizationsDictionery.ContainsKey(knownLangs[i]))
					this._localizationsDictionery.Add(knownLangs[i], new Dictionary<string, string>());
			}

			// Seting default launguages
			if (_choosenLocalization.Equals (String.Empty)) // Default localization wos never set before. 
			{
				SelectLocalization (0);
			} 
			else // In case of localization names changed.  
			{
				bool contaisValue = TableHelper.ContainValue<string> (_availableLocales, _choosenLocalization);
				if (!contaisValue) 
				{
					SelectLocalization (0);
				}
			}
		}

		private void GenerateDictionary(string[] lines, string currentFileName, string[] valueSeparatorsTab)
		{
			for(int i = 1; i < lines.Length; i++)
			{
				string[] valuesFromLine = lines [i].Split (valueSeparatorsTab, StringSplitOptions.None);

				for (int j = 1; j < valuesFromLine.Length; j++) 
				{
					Dictionary<string, string> localizationDictionery;
					bool containsValue = _localizationsDictionery.TryGetValue (_availableLocales [j - 1], out localizationDictionery);
					if (containsValue) 
					{
						string localizationKey = valuesFromLine [0];
						string localizationValue = "";
						bool containsLocalization = localizationDictionery.TryGetValue (localizationKey, out localizationValue);
						if (containsLocalization) 
						{
							Debug.LogWarningFormat ("File {1} contains localization key {0} that already exists. Use other localization key.", localizationKey, currentFileName);
						} 
						else 
						{
							localizationValue = valuesFromLine [j];
							localizationDictionery.Add (localizationKey, localizationValue);
						}
					}
				}
			}
		}

		/// <summary>
		/// Loads the locales and parse the localization csv file.
		/// </summary>
		public void LoadLocales()
	    {
			List<string> localizationFilesContents = LoadFiles ();

			int localizationFilesContentsLenght = localizationFilesContents.Count;
			for (int k = 0; k < localizationFilesContentsLenght; k++) 
			{
				string sourceFileName = localizationResourceFileNamesList [k];
				string localizationFileContents = localizationFilesContents[k];
				string [] valueSeparatorsTab = { _valueSeparator };
				string[] lines = ExtractLines (localizationFileContents, valueSeparatorsTab);
				GenerateDictionary (lines, sourceFileName, valueSeparatorsTab);
			}
	    }

	    #endregion

	}
}