using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.SceneManagment
{
    [CreateAssetMenu(fileName = "SceneSet",
      menuName = "SceneManagment/SceneSet",
      order = 1)]
    public class SceneSet : ScriptableObject
    {
        [SerializeField]
        private List<SceneInfo> _sceneInfoList = new List<SceneInfo>();
        public List<SceneInfo> SceneInfoList
        {
            get { return _sceneInfoList; }
        }
    }
}