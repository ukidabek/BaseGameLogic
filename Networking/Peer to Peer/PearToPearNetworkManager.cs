using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using BaseGameLogic.Networking;

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

        [SerializeField]
        protected List<string> _logs = new List<string>();

        protected int port = 0;
        protected byte error = 0;
        protected string adres = string.Empty;
        protected int connectionId = 0;
        protected byte[] recBuffer = null;

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

        protected virtual void HandleNewConnection()
        {
            if(NetworkTransport.IsBroadcastDiscoveryRunning())
            {
                NetworkTransport.StopBroadcastDiscovery();
            }

            NetworkID networkID;
            NodeID node;

            NetworkTransport.GetConnectionInfo(
                hostID,
                connectionId,
                out adres,
                out port,
                out networkID,
                out node,
                out error);

            if (newPear != null && newPear.ConnectionID == connectionId)
                return;


            NetworkError networkError = NetworkUtility.GetNetworkError(error);
            if(networkError == NetworkError.Ok)
            {
                newPear = new PeerInfo(adres, port);

                string log = string.Format(
                    PeerToPearNetworkManagerLogs.NEW_CONNECTION_APPEARED,
                    newPear.IPAdres,
                    newPear.Port,
                    System.DateTime.Now.ToString());

                _logs.Add(log);

                newPear.ConnectionID = connectionId;
                _connectedPeers.Add(newPear);
            }
        }

        protected virtual void HandleBrodcastNewConnection()
        {
            NetworkTransport.GetBroadcastConnectionInfo(hostID, out adres, out port, out error);
            newPear = new PeerInfo(adres, port);

            string log = string.Format(
                PeerToPearNetworkManagerLogs.NEW_PEER_TRY_TO_CONNECT,
                newPear.IPAdres,
                newPear.Port,
                System.DateTime.Now.ToString());

            _logs.Add(log);

            log = string.Format(
                PeerToPearNetworkManagerLogs.CONNECTING_TO_PEER,
                newPear.IPAdres,
                newPear.Port,
                System.DateTime.Now.ToString());

            _logs.Add(log);

            newPear.ConnectionID = NetworkTransport.Connect(hostID, adres, port, 0, out error);

            NetworkError networkError = NetworkUtility.GetNetworkError(error);

            if (networkError == NetworkError.Ok)
            {
                log = string.Format(
                    PeerToPearNetworkManagerLogs.CONNECTING_TO_PEER_SUCCEEDED,
                    newPear.IPAdres,
                    newPear.Port,
                    System.DateTime.Now.ToString());
                _connectedPeers.Add(newPear);
            }
            else
            {
                log = string.Format(
                    PeerToPearNetworkManagerLogs.CONNECTING_TO_PEER_FAIL,
                    newPear.IPAdres,
                    newPear.Port,
                    System.DateTime.Now.ToString());
            }
            _logs.Add(log);
        }

        protected virtual void Update()
        {
            int recHostId;
            int channelId;
            int dataSize;

            recBuffer = new byte[_settings.BufferSize];

            NetworkEventType recData = NetworkTransport.Receive(
                out recHostId, 
                out connectionId, 
                out channelId, 
                recBuffer, 
                _settings.BufferSize, 
                out dataSize, 
                out error);

            switch (recData)
            {
                case NetworkEventType.Nothing:         //1
                    break;

				case NetworkEventType.ConnectEvent:    //2
                    HandleNewConnection();
                    break;

                case NetworkEventType.DataEvent:       //3
                    break;

                case NetworkEventType.DisconnectEvent: //4
                    break;

                case NetworkEventType.BroadcastEvent:
                    HandleBrodcastNewConnection();
                    break;
            }
        }

		int connectionIdTT;
        public void Connect()
        {
            byte error;

            NetworkTransport.StartBroadcastDiscovery(
                hostID, 
                _settings.Port,
                _broadcastCredentials.Key,
                _broadcastCredentials.Version,
                _broadcastCredentials.Subcersion, 
                null, 
                0, 
                _broadcastCredentials.Timeout, 
                out error);
        }
    }
}
