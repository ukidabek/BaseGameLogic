using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.States
{
    [Serializable]
    public class StatTreeNode
    {
        [SerializeField]
        public List<int> _outgoingNodes = new List<int>();
    }
}