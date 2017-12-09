using UnityEngine;
using UnityEngine.SceneManagement;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Singleton;

namespace BaseGameLogic.SceneManagement
{
    public abstract class SaveLoadManager : Singleton<SaveLoadManager>
    {
        public Action GameLoadedCallBack = null;

        public SceneSet SceneSet = null;
        public bool LoadGameSave = false;

        private AsyncOperation _loadOperation = null;
        public int SceneToLoadCount
        {
            get
            {
                if(SceneSet == null)
                {
                    return -1;
                }

                return SceneSet.SceneInfoList.Count;
            }
        }

        public int _scentToLoadIndex = 0; 

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
            //LoadGame();
        }

        private void Update()
        {
            if(_loadOperation != null)
            {
                if(_loadOperation.isDone)
                {
                    if(_scentToLoadIndex == SceneToLoadCount - 1)
                    {
                        gameObject.SetActive(false);
                        if (GameLoadedCallBack != null)
                        {
                            GameLoadedCallBack();
                        }
                    }
                    else
                    {
                        _scentToLoadIndex++;
                        StartSceneLoading();
                    }
                }
            }
        }

        public void LoadGame()
        {
            this.gameObject.SetActive(true);
            _scentToLoadIndex = 0;
            StartSceneLoading();
        }

        public void StartSceneLoading()
        {
            string sceneName = SceneSet.SceneInfoList[_scentToLoadIndex].SceneName;
            LoadSceneMode mode = _scentToLoadIndex == 0 ? LoadSceneMode.Single : LoadSceneMode.Additive;
            _loadOperation = SceneManager.LoadSceneAsync(sceneName, mode);
        }

        public void SaveGame()
        {

        }
    }
}
