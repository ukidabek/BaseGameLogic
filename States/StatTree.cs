using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.States
{
    [Serializable]
    public class StatTree
    {
        public List<StatTreeNode> nodes = new List<StatTreeNode>();
    }
}