using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

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
        [SerializeField, Header("Network settings.")]
        protected PeerToPearNetworkManagerSettings _settings = new PeerToPearNetworkManagerSettings();

        [SerializeField]
        protected BroadcastCredentials _broadcastCredentials = new BroadcastCredentials();

        [SerializeField]
        protected MatchSettings matchSettings = new MatchSettings();

        // Match making.
        protected List<MatchInfoSnapshot> m_MatchList = new List<MatchInfoSnapshot>();
        protected MatchInfo matchInfo;

        [SerializeField, Header("Match settings.")]
        protected bool matchCreated;
        [SerializeField]
        protected bool matchJoined;
        [SerializeField]
        protected NetworkMatch networkMatch;

        [SerializeField, Header("Run time values."), Tooltip("List of connected peers. Do not setup!")]
        protected List<PeerInfo> _connectedPeers = new List<PeerInfo>();

        [SerializeField]
        protected int hostID = 0;

        [SerializeField]
        protected List<string> _logs = new List<string>();

        [SerializeField]
        protected int port = 0;
        [SerializeField]
        protected byte error = 0;
        [SerializeField]
        protected string adres = string.Empty;
        [SerializeField]
        protected int connectionID = 0;
        [SerializeField]
        protected int recHostId;
        [SerializeField]
        protected int channelId;
        [SerializeField]
        protected int dataSize;
        [SerializeField]
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

            this.enabled = true;
        }

        public virtual void StartSession()
        {
            //_settings.PearType = PeerToPeerNetworkManagerEnum.MasterPear;

            if (SaveLoadManager.Instance != null)
            {
                SaveLoadManager.Instance.GameLoadedCallBack -= CreateMatch;
                SaveLoadManager.Instance.GameLoadedCallBack += CreateMatch;
            }

            //Initialize();
        }

        public virtual void JoinSession()
        {
            networkMatch.ListMatches(0,1,"",true,0,0, (success, info, matches) =>
            {
                if (success && matches.Count > 0)
                {
                    networkMatch.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnMatchJoined);
                }
            });
            //_settings.PearType = PeerToPeerNetworkManagerEnum.Pear;
            //Initialize();
            //Connect();
        }

        protected void AddChanel(ref ConnectionConfig connectionConfig, QosType type)
        {
            int channelId = connectionConfig.AddChannel(type);
            channelDictionary.Add(type, channelId);
        }

        protected override void Awake()
        {
            base.Awake();

            networkMatch = gameObject.AddComponent<NetworkMatch>();

            this.enabled = false;
            //Initialize();

            // Make sure if connected pears list is empty
            _connectedPeers.Clear();
        }

        public virtual void Start() {}

        protected virtual void OnDestroy()
        {
            if (SaveLoadManager.Instance != null)
            {
                SaveLoadManager.Instance.GameLoadedCallBack -= Connect;
            }
        }

        public virtual void SetPeerType(PeerToPeerNetworkManagerEnum type)
        {
            _settings.PearType = type;
        }

        protected virtual void OnApplicationQuit()
        {
            NetworkTransport.Shutdown();
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
            if(hostID == -1)
            {
                this.enabled = false;
                return;
            }

            recBuffer = new byte[_settings.BufferSize];

            // Get events from the relay connection
            NetworkEventType networkEvent = NetworkTransport.ReceiveRelayEventFromHost(hostID, out error);

            if (networkEvent == NetworkEventType.ConnectEvent)
                Debug.Log("Relay server connected");

            if (networkEvent == NetworkEventType.DisconnectEvent)
                Debug.Log("Relay server disconnected");

            do
            {
                // Get events from the server/client game connection
                networkEvent = NetworkTransport.ReceiveFromHost(
                    hostID, 
                    out connectionID, 
                    out channelId,
                    recBuffer, 
                    recBuffer.Length, 
                    out dataSize, 
                    out error);

                if ((NetworkError)error != NetworkError.Ok)
                {
                    Debug.LogError("Error while receiving network message: " + (NetworkError)error);
                }

                switch (networkEvent)
                {
                    case NetworkEventType.ConnectEvent:
                        HandleConnection();
                        break;

                    case NetworkEventType.DataEvent:
                        HandleMessages(recBuffer, dataSize);
                        break;

                    case NetworkEventType.DisconnectEvent:
                        HandleDisconnection();
                        break;

                    case NetworkEventType.Nothing:
                        break;
                }
            } while (networkEvent != NetworkEventType.Nothing);

            //        networkEvent = NetworkTransport.Receive(
            //            out recHostId, 
            //            out connectionID, 
            //            out channelId, 
            //            recBuffer, 
            //            _settings.BufferSize, 
            //            out dataSize, 
            //            out error);

            //        switch (networkEvent)
            //        {
            //            case NetworkEventType.Nothing:         //1
            //                break;

            //case NetworkEventType.ConnectEvent:    //2
            //                HandleConnection();
            //                break;

            //            case NetworkEventType.DataEvent:       //3
            //                HandleMessages(recBuffer, dataSize);
            //                break;

            //            case NetworkEventType.DisconnectEvent: //4
            //                HandleDisconnection();
            //                break;

            //            case NetworkEventType.BroadcastEvent:
            //                HandleBrodcastNewConnection();
            //                break;
            //        }
        }

        public virtual void Connect()
        {
            byte error = 0;
            if(gameObject.activeSelf != true)
            {
                gameObject.SetActive(true);
            }

            switch(_settings.PearType)
            {
                case PeerToPeerNetworkManagerEnum.MasterPear:
                    //  Master setup the broadcast settings, for incoming connection handling.
                    NetworkTransport.SetBroadcastCredentials(
                        hostID,
                        _broadcastCredentials.Key,
                        _broadcastCredentials.Version,
                        _broadcastCredentials.Subversion,
                        out error);
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

        // Match making 
        public virtual void CreateMatch()
        {
            if (networkMatch != null)
            {
                networkMatch.CreateMatch(
                    matchSettings.MatchName,
                    matchSettings.MatchSize,
                    matchSettings.MatchAdresatice,
                    "",
                    "",
                    "",
                    0,
                    0,
                    OnMatchCreate);
            }
        }

        public virtual void OnMatchCreate(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (success)
            {
                Debug.Log("Create match succeeded");
                Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);

                matchCreated = true;
                this.matchInfo = matchInfo;

                StartServer(matchInfo.address, matchInfo.port, matchInfo.networkId,
                    matchInfo.nodeId);
            }
            else
            {
                Debug.LogError("Create match failed: " + extendedInfo);
            }
        }

        public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matches)
        {
            if (success && matches != null)
            {
                m_MatchList = matches;
            }
            else if (!success)
            {
                Debug.LogError("List match failed: " + extendedInfo);
            }
        }

        // When we've joined a match we connect to the server/host
        public virtual void OnMatchJoined(bool success, string extendedInfo, MatchInfo matchInfo)
        {
            if (success)
            {
                Debug.Log("Join match succeeded");
                Utility.SetAccessTokenForNetwork(matchInfo.networkId, matchInfo.accessToken);

                matchJoined = true;
                this.matchInfo = matchInfo;

                Debug.Log(
                    "Connecting to Address:" + matchInfo.address +
                    " Port:" + matchInfo.port +
                    " NetworKID: " + matchInfo.networkId +
                    " NodeID: " + matchInfo.nodeId);

                ConnectThroughRelay(
                    matchInfo.address, 
                    matchInfo.port, 
                    matchInfo.networkId,
                    matchInfo.nodeId);
            }
            else
            {
                Debug.LogError("Join match failed: " + extendedInfo);
            }
        }


        public virtual void StartServer(string relayIP, int relayPort, NetworkID networkID, NodeID nodeID)
        {
            _settings.PearType = PeerToPeerNetworkManagerEnum.MasterPear;
            Initialize();

            SourceID sourceID = Utility.GetSourceID();
            NetworkTransport.ConnectAsNetworkHost(
                hostID, 
                relayIP, 
                relayPort, 
                networkID, 
                sourceID, 
                nodeID, 
                out error);
        }

        public virtual void ConnectThroughRelay(string relayIp, int relayPort, NetworkID networkId, NodeID nodeId)
        {
            _settings.PearType = PeerToPeerNetworkManagerEnum.Pear;
            Initialize();

            SourceID sourceID = Utility.GetSourceID();
            NetworkTransport.ConnectToNetworkPeer(
                hostID, 
                relayIp, 
                relayPort, 
                0, 
                0, 
                networkId, 
                sourceID, 
                nodeId, 
                out error);
        }
    }
}
