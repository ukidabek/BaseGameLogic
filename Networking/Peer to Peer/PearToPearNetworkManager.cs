﻿using UnityEngine;
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
        public static PeerToPearNetworkManager Instance { get; protected set; }

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
        protected MemoryStream memeoryStream = null;

        protected virtual void Initialize()
        {
            // Transport layer initialization.
            NetworkTransport.Init();

            // Conection configuration.
            ConnectionConfig config = new ConnectionConfig();

            AddChanel(ref config, QosType.Reliable);
            AddChanel(ref config, QosType.UnreliableSequenced);

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

        public virtual void Awake()
        {
            Initialize();
            if(Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(this);
                return;
            }

            // Make sure if connected pears list is empty
            _connectedPeers.Clear();
        }

        public virtual void Start() {}

        protected virtual bool NewPearFromBroadcastConedted(int connectionId)
        {
            if (newPear != null && newPear.ConnectionID == connectionId)
            {
                SendPeersList();
                return true;
            }

            return false;
        }

        protected virtual void NewPeerConected(int connectionId) {}

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

            if (NewPearFromBroadcastConedted(connectionId))
            {
                return;
            }

            string log = string.Format(
                PeerToPearNetworkManagerLogs.NEW_CONNECTION_APPEARED,
                NetworkUtility.GetIPAdress(adres),
                port,
                System.DateTime.Now.ToString());

            _logs.Add(log);

            NetworkError networkError = NetworkUtility.GetNetworkError(error);
            if(networkError == NetworkError.Ok)
            {
                newPear = new PeerInfo(adres, port);

                log = string.Format(
                    PeerToPearNetworkManagerLogs.CONNECTING_TO_PEER_SUCCEEDED,
                    newPear.IPAdres,
                    newPear.Port,
                    System.DateTime.Now.ToString());

                _logs.Add(log);

                newPear.ConnectionID = connectionId;
                _connectedPeers.Add(newPear);

                NewPeerConected(newPear.ConnectionID);
            }
        }

        protected virtual Message HandleMessages(byte[] buffer, int sieze)
        {
            memeoryStream = new MemoryStream(recBuffer);
            memeoryStream.Position = 0;
            Message message = (Message)binaryFormatter.Deserialize(memeoryStream);
            
            switch(message.MessageID)
            {
                case PeerToPeerMessageID.PEAR_LIST:
                    List<PeerInfo> peerList = (List<PeerInfo>)message.Data;
                    _connectedPeers.AddRange(peerList);
                    for (int i = 0; i < peerList.Count; i++)
                    {
                        PeerInfo peer = peerList[i];
                        ConnectToPear(ref peer);
                    }
                    return null;
            }

            return message;
        }

        protected virtual NetworkError ConnectToPear(ref PeerInfo peer)
        {
            peer.ConnectionID = NetworkTransport.Connect(
                hostID,
                peer.IPAdres,
                peer.Port,
                0,
                out error);

            return NetworkUtility.GetNetworkError(error);
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

            NetworkError networkError = ConnectToPear(ref newPear);

            if (networkError == NetworkError.Ok)
            {
                log = string.Format(
                    PeerToPearNetworkManagerLogs.CONNECTING_TO_PEER_SUCCEEDED,
                    newPear.IPAdres,
                    newPear.Port,
                    System.DateTime.Now.ToString());

                NewPeerConected(newPear.ConnectionID);
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

        protected void SendPeersList()
        {
            Message message = new Message(PeerToPeerMessageID.PEAR_LIST);

            SendReliable(message, newPear.ConnectionID);

            _connectedPeers.Add(newPear);
        }

        protected virtual NetworkError SendReliable(Message message, int connectionId)
        {
            message.Data = _connectedPeers;
            memeoryStream = new MemoryStream();
            binaryFormatter.Serialize(memeoryStream, message);
            byte[] arry = memeoryStream.ToArray();

            NetworkTransport.Send(
                hostID,
                connectionId,
                channelDictionary[QosType.Reliable],
                arry,
                arry.Length,
                out error);

            return NetworkUtility.GetNetworkError(error);
        }

        protected virtual NetworkError UpdateUnreiable(Message message, int connectionId)
        {
            memeoryStream = new MemoryStream();
            binaryFormatter.Serialize(memeoryStream, message);
            byte[] array = memeoryStream.ToArray();

            NetworkTransport.Send(
                hostID,
                connectionId,
                channelDictionary[QosType.UnreliableSequenced],
                array,
                array.Length,
                out error);

            return NetworkUtility.GetNetworkError(error);
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
                    HandleMessages(recBuffer, dataSize);
                    break;

                case NetworkEventType.DisconnectEvent: //4
                    break;

                case NetworkEventType.BroadcastEvent:
                    HandleBrodcastNewConnection();
                    break;
            }
        }

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
