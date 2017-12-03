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
        protected bool initializationOnAwake = false;
        public bool InitializationOnAwake
        {
            get { return initializationOnAwake; }
        }

        [SerializeField]
        protected PeerToPeerNetworkManagerEnum _pearType = PeerToPeerNetworkManagerEnum.MasterPear;
        public PeerToPeerNetworkManagerEnum PearType
        {
            get { return _pearType; }
            set { _pearType = value; }
        }

    }
}