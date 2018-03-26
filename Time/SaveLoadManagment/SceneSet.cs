using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.SceneManagement
{
    public abstract class BaseSceneSet : ScriptableObject
    {
        [SerializeField]
        private List<SceneInfo> _sceneInfoList = new List<SceneInfo>();
        public List<SceneInfo> SceneInfoList
        {
            get { return _sceneInfoList; }
        }
    }
}