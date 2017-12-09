using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

using BaseGameLogic.Singleton;
using BaseGameLogic.SceneManagement;

namespace BaseGameLogic.Networking.PeerToPeer
{
    public abstract class PeerToPearNetworkManager : Singleton<PeerToPearNetworkManager>
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
        protected int connectionID = 0;
        protected byte[] recBuffer = null;

        protected PeerInfo newPear = null;

        protected Dictionary<QosType, int> channelDictionary = new Dictionary<QosType, int>();
        protected BinaryFormatter binaryFormatter = new BinaryFormatter();
        protected MemoryStream memoryStream = null;

        protected virtual void Initialize()
        {
            // Transport layer initialization.
            NetworkTransport.Init();

            // Connection configuration.
            ConnectionConfig config = new ConnectionConfig();

            AddChanel(ref config, QosType.Reliable);
            AddChanel(ref config, QosType.UnreliableSequenced);

            // Topology configuration.
            HostTopology topology = new HostTopology(config, _settings.ConnectionsCount);

            // Set get port settings. If master use value for settings if not use first free port.
            int portToUse = _settings.PearType == PeerToPeerNetworkManagerEnum.MasterPear ? _settings.Port : 0;
            hostID = NetworkTransport.AddHost(topology, portToUse);

            this.enabled = false;
        }

        public virtual void StartSession()
        {
            _settings.PearType = PeerToPeerNetworkManagerEnum.MasterPear;
            Initialize();
        }

        public virtual void JoinSession()
        {
            _settings.PearType = PeerToPeerNetworkManagerEnum.Pear;
            Initialize();
        }

        protected void AddChanel(ref ConnectionConfig connectionConfig, QosType type)
        {
            int channelId = connectionConfig.AddChannel(type);
            channelDictionary.Add(type, channelId);
        }

        protected override void Awake()
        {
            base.Awake();
            this.enabled = false;
            //Initialize();

            // Make sure if connected pears list is empty
            _connectedPeers.Clear();
        }

        public virtual void Start()
        {
            //if (SaveLoadManager.Instance != null)
            //{
            //    SaveLoadManager.Instance.GameLoadedCallBack -= Connect;
            //    SaveLoadManager.Instance.GameLoadedCallBack += Connect;
            //}
        }

        protected virtual void OnDestroy()
        {
            //if (SaveLoadManager.Instance != null)
            //{
            //    SaveLoadManager.Instance.GameLoadedCallBack -= Connect;
            //}
        }

        public virtual void SetPeerType(PeerToPeerNetworkManagerEnum type)
        {
            _settings.PearType = type;
        }

        protected virtual void PeerConnected(int connectionId) {}

        protected virtual bool NewPearFromBroadcastConedted(int connectionId)
        {
            if (newPear != null && newPear.ConnectionID == connectionId)
            {
                SendPeersList();
                return true;
            }

            return false;
        }

        protected virtual void HandleConnection()
        {
            if(NetworkTransport.IsBroadcastDiscoveryRunning())
            {
                NetworkTransport.StopBroadcastDiscovery();
            }

            NetworkID networkID;
            NodeID node;

            NetworkTransport.GetConnectionInfo(
                hostID,
                connectionID,
                out adres,
                out port,
                out networkID,
                out node,
                out error);

            if (NewPearFromBroadcastConedted(connectionID))
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

                newPear.ConnectionID = connectionID;
                _connectedPeers.Add(newPear);

                PeerConnected(newPear.ConnectionID);
            }
        }

        protected virtual void PeerDisconnected(int connectionID) { }

        protected virtual void HandleDisconnection()
        {
            for (int i = 0; i < _connectedPeers.Count; i++)
            {
                PeerInfo info = _connectedPeers[i];
                if(info.ConnectionID == connectionID)
                {
                    _connectedPeers.RemoveAt(i);
                    break;
                }
            }

            PeerDisconnected(connectionID);
        }

        protected virtual Message HandleMessages(byte[] buffer, int sieze)
        {
            memoryStream = new MemoryStream(recBuffer);
            memoryStream.Position = 0;
            Message message = (Message)binaryFormatter.Deserialize(memoryStream);
            message.ConnectionID = connectionID;

            switch (message.MessageID)
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

                PeerConnected(newPear.ConnectionID);
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

        protected virtual void SendPeersList()
        {
            Message message = new Message(PeerToPeerMessageID.PEAR_LIST);

            message.Data = _connectedPeers;
            SendReliable(message, newPear.ConnectionID);

            _connectedPeers.Add(newPear);
        }

        protected virtual NetworkError SendReliable(Message message, int connectionId)
        {
            memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, message);
            byte[] arry = memoryStream.ToArray();

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
            memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, message);
            byte[] array = memoryStream.ToArray();

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
                out connectionID, 
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
                    HandleConnection();
                    break;

                case NetworkEventType.DataEvent:       //3
                    HandleMessages(recBuffer, dataSize);
                    break;

                case NetworkEventType.DisconnectEvent: //4
                    HandleDisconnection();
                    break;

                case NetworkEventType.BroadcastEvent:
                    HandleBrodcastNewConnection();
                    break;
            }
        }

        public virtual void Connect()
        {
            byte error = 0;

            switch(_settings.PearType)
            {
                case PeerToPeerNetworkManagerEnum.MasterPear:
                    //  Master setup the broadcast settings, for incoming connection handling.
                    if (_settings.PearType == PeerToPeerNetworkManagerEnum.MasterPear)
                    {
                        NetworkTransport.SetBroadcastCredentials(
                        hostID,
                        _broadcastCredentials.Key,
                        _broadcastCredentials.Version,
                        _broadcastCredentials.Subversion,
                        out error);
                    }
                    break;

                case PeerToPeerNetworkManagerEnum.Pear:
                    NetworkTransport.StartBroadcastDiscovery(
                        hostID,
                        _settings.Port,
                        _broadcastCredentials.Key,
                        _broadcastCredentials.Version,
                        _broadcastCredentials.Subversion,
                        null,
                        0,
                        _broadcastCredentials.Timeout,
                        out error);
                    break;
            }
        }
    }
}
