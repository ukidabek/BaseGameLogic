using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking.PeerToPeer
{
    [Serializable]
    public class PeerToPearNetworkManagerSettings
    {
        [SerializeField]
        private int _port = 8888;
        public int Port { get { return _port; } }

        [SerializeField]
        protected int _connectionsCount = 8;
        public int ConnectionsCount { get { return _connectionsCount; } }

        [SerializeField]
        protected PeerToPeerNetworkManagerEnum _pearType = PeerToPeerNetworkManagerEnum.MasterPear;
        public PeerToPeerNetworkManagerEnum PearType { get { return _pearType; } }
    }
}