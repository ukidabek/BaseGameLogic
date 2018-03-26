using UnityEditor;
using UnityEngine;

using System;

namespace BaseGameLogic.SceneManagement
{
    public class SaveLoadManagerEditor : Editor
    {
        [MenuItem("BaseGameLogic/SaveLoad/Create SaveLoadManager")]
        public static BaseSaveLoadManager CreateSaveLoadManager()
		{
            return GameObjectExtension.CreateInstanceOfAbstractType<BaseSaveLoadManager>();
        }
    }
}