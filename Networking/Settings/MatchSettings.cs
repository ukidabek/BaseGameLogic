using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking
{
    [Serializable]
    public class MatchSettings
    {
        [SerializeField]
        private string matchName = "NewRoom";
        public string MatchName
        {
            get { return matchName; }
            set { matchName = value; }
        }

        [SerializeField]
        private uint matchSize = 8;
        public uint MatchSize
        {
            get { return matchSize; }
            set { matchSize = value; }
        }
        
        [SerializeField]
        private bool matchAdresatice = true;
        public bool MatchAdresatice
        {
            get { return matchAdresatice; }
            set { matchAdresatice = value; }
        }
    }
}