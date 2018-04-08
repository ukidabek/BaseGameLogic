using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.SceneManagement
{
    public abstract class BaseSceneSet : ScriptableObject
    {
        [SerializeField, HideInInspector]
        private List<SceneInfo> _sceneInfoList = new List<SceneInfo>();

        public SceneInfo this [int index]
        {
            get { return _sceneInfoList[index]; }
            set { _sceneInfoList[index] = value; }
        }

        public int Count
        {
             get { return _sceneInfoList.Count; }
        }

        public void Add(SceneInfo sceneInfo)
        {
            _sceneInfoList.Add(sceneInfo);
        }
    }
}