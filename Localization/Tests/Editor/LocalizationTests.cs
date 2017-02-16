using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;



namespace BaseGameLogic.Localization
{
	internal class LocalizationTests
	{
		LocalizationManager DummyLocalizationManager  = new LocalizationManager( new List<string> {"locales.csv"}, "\r\n", ";");		

		[Test]
		public void GetTranslationTest ()
		{
			DummyLocalizationManager.SetDefaultLocalization ();
			DummyLocalizationManager.LoadLocales ();
			string key = "Demo_NavMesh_Dialogue_0";
			string translation = null;
			for (int i = 0; i < DummyLocalizationManager.AvailableLocales.Length; i++) {
				
				DummyLocalizationManager.SelectLocalization (i);
				translation = DummyLocalizationManager.GetTranslation (key);
				Assert.IsNotNull (translation);
			}

		}
	}
}

