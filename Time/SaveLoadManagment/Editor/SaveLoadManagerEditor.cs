using UnityEditor;
using UnityEngine;

using System;

namespace BaseGameLogic.SceneManagement
{
    public class SaveLoadManagerEditor : Editor
    {
        [MenuItem("BaseGameLogic/SaveLoad/Create SaveLoadManager")]
        public static SaveLoadManager CreateSaveLoadManager()
		{
            return GameObjectExtension.CreateInstanceOfAbstractType<SaveLoadManager>();
        }
    }
}