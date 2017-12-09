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

        public int MapToLoadIndex = 0; 
        [SerializeField]
        private List<SceneSet> _sceneSetList = new List<SceneSet>();
        public SceneSet SceneSet { get { return _sceneSetList[MapToLoadIndex]; } }

        public bool LoadGameSave = false;

        private int CurrentLoadingSceneIndex = 0;

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

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);
        }

        private void Update()
        {
            if(_loadOperation != null)
            {
                if(_loadOperation.isDone)
                {
                    if(CurrentLoadingSceneIndex == SceneToLoadCount - 1)
                    {
                        gameObject.SetActive(false);
                        if (GameLoadedCallBack != null)
                        {
                            GameLoadedCallBack();
                        }
                    }
                    else
                    {
                        CurrentLoadingSceneIndex++;
                        StartSceneLoading();
                    }
                }
            }
        }

        public void LoadGame()
        {
            this.gameObject.SetActive(true);
            CurrentLoadingSceneIndex = 0;
            StartSceneLoading();
        }

        public void StartSceneLoading()
        {
            string sceneName = SceneSet.SceneInfoList[CurrentLoadingSceneIndex].SceneName;
            LoadSceneMode mode = CurrentLoadingSceneIndex == 0 ? LoadSceneMode.Single : LoadSceneMode.Additive;
            _loadOperation = SceneManager.LoadSceneAsync(sceneName, mode);
        }

        public void SaveGame() { }
    }
}
