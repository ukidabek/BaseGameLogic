using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Singleton;

namespace BaseGameLogic.SceneManagement
{
    public abstract class SaveLoadManager : Singleton<SaveLoadManager>
    {
        [SerializeField]
        private Camera _loadingScreenCamera = null;

        [SerializeField]
        private LoadingScreenCanvas _loadingScreenCanvas = null;

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

        [SerializeField, Range(0f, 1f)]
        private float _loadingProgress = 0;

        [SerializeField]
        private float _progressPerScene = 0;

        protected override void Awake()
        {
            base.Awake();

            gameObject.SetActive(false);

            if (_loadingScreenCanvas != null)
            {
                _loadingScreenCanvas.gameObject.SetActive(false);
            }

            if (_loadingScreenCamera != null)
            {
                _loadingScreenCamera.gameObject.SetActive(false);
            }

        }

        private void Update()
        {
            if(_loadOperation != null)
            {
                if(_loadOperation.isDone)
                {
                    if(_loadingScreenCanvas != null)
                    {
                        _loadingScreenCanvas.LoadingProgress = _loadingProgress + (_progressPerScene * _loadOperation.progress);
                        Debug.Log(_loadOperation.progress);
                    }

                    if(CurrentLoadingSceneIndex == SceneToLoadCount - 1)
                    {
                        gameObject.SetActive(false);

                        if (_loadingScreenCanvas != null)
                        {
                            _loadingScreenCanvas.gameObject.SetActive(false);
                        }

                        if(_loadingScreenCamera != null)
                        {
                            _loadingScreenCamera.gameObject.SetActive(false);
                        }

                        if (GameLoadedCallBack != null)
                        {
                            CurrentLoadingSceneIndex = 0;
                            GameLoadedCallBack();
                        }
                    }
                    else
                    {
                        CurrentLoadingSceneIndex++;
                        _loadingProgress += _progressPerScene;
                        StartSceneLoading();
                    }
                }
            }
        }

        public void LoadGame()
        {
            this.gameObject.SetActive(true);
            CurrentLoadingSceneIndex = 0;

            _progressPerScene = 1f / SceneToLoadCount;

            if (_loadingScreenCanvas != null)
            {
                _loadingScreenCanvas.gameObject.SetActive(true);
            }

            if (_loadingScreenCamera != null)
            {
                _loadingScreenCamera.gameObject.SetActive(true);
            }

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
