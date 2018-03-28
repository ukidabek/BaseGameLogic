﻿using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Singleton;
using BaseGameLogic.Networking;

namespace BaseGameLogic.SceneManagement
{
    public abstract class BaseSceneLoadManager : Singleton<BaseSceneLoadManager>
    {
        [Space]
        public int MapToLoadIndex = 0; 

        [SerializeField, HideInInspector]
        private List<BaseSceneSet> _sceneSetList = new List<BaseSceneSet>();
        public BaseSceneSet SceneSet { get { return _sceneSetList[MapToLoadIndex]; } }

        private int CurrentLoadingSceneIndex = 0;

        private AsyncOperation _loadOperation = null;

        public int SceneToLoadCount
        {
            get
            {
                if(SceneSet == null)
                    return -1;

                return SceneSet.SceneInfoList.Count;
            }
        }

        [SerializeField, Range(0f, 1f)]
        private float _loadingProgress = 0;
        private float _progressPerScene = 0;

        public UnityEvent LoadingStart = new UnityEvent();
        public LoadingProgressUpdate LoadingProgressUpdate = new LoadingProgressUpdate();
        public UnityEvent LoadingEnd = new UnityEvent();

        private void Update()
        {
            if(_loadOperation != null)
            {
                if(_loadOperation.isDone)
                {
                    LoadingProgressUpdate.Invoke(_loadingProgress + (_progressPerScene * _loadOperation.progress));

                    if(CurrentLoadingSceneIndex == SceneToLoadCount - 1)
                    {
                        enabled = false;
                        LoadingEnd.Invoke();
                        CurrentLoadingSceneIndex = 0;
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

        public void LoadLevel()
        {
            enabled = true;
            CurrentLoadingSceneIndex = 0;

            _progressPerScene = 1f / SceneToLoadCount;
            LoadingStart.Invoke();
            StartSceneLoading();
        }

        public void StartSceneLoading()
        {
            string sceneName = SceneSet.SceneInfoList[CurrentLoadingSceneIndex].SceneName;
            LoadSceneMode mode = CurrentLoadingSceneIndex == 0 ? LoadSceneMode.Single : LoadSceneMode.Additive;
            _loadOperation = SceneManager.LoadSceneAsync(sceneName, mode);
        }
    }

    [Serializable]
    public sealed class LoadingProgressUpdate : UnityEvent<float> {}
}
