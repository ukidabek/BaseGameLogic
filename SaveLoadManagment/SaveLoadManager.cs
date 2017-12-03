using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Singleton;

namespace BaseGameLogic.SceneManagment
{
    public abstract class SaveLoadManager : Singleton<SaveLoadManager>
    {
        public Action GameLoadedCallBack = null;

        public SceneSet SceneSet = null;
        public bool LoadGameSave = false;

        public void LoadGame()
        {
            //SceneManager.load
        }

        public void SaveGame()
        {

        }
    }
}
