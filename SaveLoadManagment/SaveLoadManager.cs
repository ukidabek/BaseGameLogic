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

        public int ScentToLoadIndex = 0; 
        [SerializeField]
        private List<SceneSet> _sceneSetList = new List<SceneSet>();
        public SceneSet SceneSet { get { return _sceneSetList[ScentToLoadIndex]; } }

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
                    if(ScentToLoadIndex == SceneToLoadCount - 1)
                    {
                        gameObject.SetActive(false);
                        if (GameLoadedCallBack != null)
                        {
                            GameLoadedCallBack();
                        }
                    }
                    else
                    {
                        ScentToLoadIndex++;
                        StartSceneLoading();
                    }
                }
            }
        }

        public void LoadGame()
        {
            this.gameObject.SetActive(true);
            ScentToLoadIndex = 0;
            StartSceneLoading();
        }

        public void StartSceneLoading()
        {
            string sceneName = SceneSet.SceneInfoList[ScentToLoadIndex].SceneName;
            LoadSceneMode mode = ScentToLoadIndex == 0 ? LoadSceneMode.Single : LoadSceneMode.Additive;
            _loadOperation = SceneManager.LoadSceneAsync(sceneName, mode);
        }

        public void SaveGame() { }
    }
}
