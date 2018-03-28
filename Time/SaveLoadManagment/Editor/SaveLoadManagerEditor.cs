using UnityEditor;
using UnityEngine;

using System;

namespace BaseGameLogic.SceneManagement
{
    public class SaveLoadManagerEditor : Editor
    {
        [MenuItem("BaseGameLogic/SaveLoad/Create SaveLoadManager")]
        public static BaseSceneLoadManager CreateSaveLoadManager()
		{
            return GameObjectExtension.CreateInstanceOfAbstractType<BaseSceneLoadManager>();
        }
    }
}