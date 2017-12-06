using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.SceneManagment
{ 
    [Serializable]
    public class SceneInfo
    {
        public int SceneIndex = 0;
        public string SceneName = string.Empty;
        public string ScenePath = string.Empty;
    }
}