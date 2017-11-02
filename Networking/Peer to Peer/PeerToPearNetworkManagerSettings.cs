using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Networking;

namespace BaseGameLogic.Networking.PeerToPeer
{
    [Serializable]
    public class PeerToPearNetworkManagerSettings : BaseNetworkManagerSettings
    {
        [SerializeField]
        protected PeerToPeerNetworkManagerEnum _pearType = PeerToPeerNetworkManagerEnum.MasterPear;
        public PeerToPeerNetworkManagerEnum PearType { get { return _pearType; } }
    }
}