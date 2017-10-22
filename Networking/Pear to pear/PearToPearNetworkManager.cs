using UnityEngine;
using UnityEngine.Networking;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking.PearToPear
{
    public abstract class PearToPearNetworkManager : MonoBehaviour
    {
        [SerializeField]
        protected int port = 8888;

        [SerializeField]
        protected int connectionsCount = 8;
        
        [SerializeField]
        protected PearToPearNetworkManagerEnum _pearType = PearToPearNetworkManagerEnum.MasterPear;

        [SerializeField]
        protected int hostID = 0;

        protected Dictionary<QosType, int> channelDictionary = new Dictionary<QosType, int>();

        public void Awake()
        {
            // Transport layer initialization.
            NetworkTransport.Init();

            // Conection configuration
            ConnectionConfig config = new ConnectionConfig();

            AddChanel(ref config, QosType.Reliable);

            // Topology configuration
            HostTopology topology = new HostTopology(config, connectionsCount);

            hostID = NetworkTransport.AddHost(topology, port);

        }

        protected void AddChanel(ref ConnectionConfig conectionConfig, QosType type)
        {
            int channelId = conectionConfig.AddChannel(type);
            channelDictionary.Add(type, channelId);
        }
    }
}
