using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BaseGameLogic.Networking.PeerToPeer
{
    public abstract class PeerToPearNetworkManager : MonoBehaviour
    {
        [SerializeField]
        protected PeerToPearNetworkManagerSettings _settings = new PeerToPearNetworkManagerSettings();

        [SerializeField]
        protected BroadcastCredentials _broadcastCredentials = new BroadcastCredentials();

        [SerializeField]
        protected int hostID = 0;

        [SerializeField, Tooltip("List of connected peers. Do not setup!")]
        protected List<PeerInfo> _connectedPeers = new List<PeerInfo>();

        protected int port = 0;
        protected byte error = 0;
        protected string adres = string.Empty;
        protected PeerInfo newPear = null;

        protected Dictionary<QosType, int> channelDictionary = new Dictionary<QosType, int>();
        protected BinaryFormatter binaryFormatter = new BinaryFormatter();

        protected virtual void Initialize()
        {
            // Transport layer initialization.
            NetworkTransport.Init();

            // Conection configuration.
            ConnectionConfig config = new ConnectionConfig();

            AddChanel(ref config, QosType.Reliable);

            // Topology configuration.
            HostTopology topology = new HostTopology(config, _settings.ConnectionsCount);

            // Set get port settings. If master use value for settings if not use first free port.
            int portToUse = _settings.PearType == PeerToPeerNetworkManagerEnum.MasterPear ? _settings.Port : 0;
            hostID = NetworkTransport.AddHost(topology, portToUse);

            // If master setup the broadcast settings, for incoming connection handling.
            if (_settings.PearType == PeerToPeerNetworkManagerEnum.MasterPear)
            {
                NetworkTransport.SetBroadcastCredentials(
                    hostID,
                    _broadcastCredentials.Key,
                    _broadcastCredentials.Version,
                    _broadcastCredentials.Subcersion,
                    out error);
            }
        }

        public void Awake()
        {
            Initialize();
            
            // Make sure if connected pears list is empty
            _connectedPeers.Clear();
        }

        protected void AddChanel(ref ConnectionConfig conectionConfig, QosType type)
        {
            int channelId = conectionConfig.AddChannel(type);
            channelDictionary.Add(type, channelId);
        }

        protected virtual void Update()
        {
            int recHostId;
            int connectionId;
            int channelId;
            byte[] recBuffer = new byte[1024];
            int bufferSize = 1024;
            int dataSize;
            byte error;
            NetworkEventType recData = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, bufferSize, out dataSize, out error);
            switch (recData)
            {
                case NetworkEventType.Nothing:         //1
                    break;
				case NetworkEventType.ConnectEvent:    //2
                    NetworkID networkID;
                    NodeID node;
                    NetworkTransport.GetConnectionInfo(hostID, connectionId, out adres, out port, out networkID, out node, out error);
                    newPear = new PeerInfo(adres, port);
                    newPear.ConnectionID = connectionId;
                    break;

                case NetworkEventType.DataEvent:       //3
                    break;

                case NetworkEventType.DisconnectEvent: //4
                    break;

                case NetworkEventType.BroadcastEvent:
                    NetworkTransport.GetBroadcastConnectionInfo(hostID, out adres, out port, out error);
                    newPear = new PeerInfo(adres, port);
                    newPear.ConnectionID = NetworkTransport.Connect(hostID, adres, port, 0, out error);
                    _connectedPeers.Add(newPear);
                    NetworkError er = (NetworkError)error;
                    Debug.Log(er);
                    break;
            }
        }

		int connectionIdTT;
        public void Connect()
        {
            byte error;

            NetworkTransport.StartBroadcastDiscovery(hostID, _settings.Port, 1, 1, 1, null, 0, 3, out error);
        }
    }
}
