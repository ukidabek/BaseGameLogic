using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking
{
    [Serializable]
    public class BroadcastCredentials
    {
        [SerializeField]
        private int _key = 1;
        public int Key { get { return _key; } }

        [SerializeField]
        private int _version = 1;
        public int Version { get { return _version; } }

        [SerializeField]
        private int _subcersion = 1;
        public int Subcersion { get { return _subcersion; } }
    }
}