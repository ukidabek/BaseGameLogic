using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;
using UnityEngine.Networking.Match;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Linq;

using BaseGameLogic.Singleton;
using BaseGameLogic.SceneManagement;

namespace BaseGameLogic.Networking
{
    public abstract class NetworkManager : Singleton<NetworkManager>
    {
        [SerializeField, Header("Network settings.")]
        protected NetworkManagerSettings _settings = new NetworkManagerSettings();

        [SerializeField]
        protected BroadcastCredentials _broadcastCredentials = new BroadcastCredentials();

        [SerializeField]
        protected MatchSettings matchSettings = new MatchSettings();

        // Match making.

        [SerializeField, Header("Match settings.")]
        protected bool matchCreated;
        [SerializeField]
        protected bool matchJoined;
        [SerializeField]
        protected NetworkMatch networkMatch;

        protected List<MatchInfoSnapshot> matchList = new List<MatchInfoSnapshot>();
        protected MatchInfo matchInfo;

        [SerializeField, Header("Run time values."), Tooltip("List of connected peers. Do not setup!")]
        protected List<PeerInfo> connectedPeers = new List<PeerInfo>();

        [SerializeField]
        protected int hostID = 0;

        [SerializeField]
        protected List<string> _logs = new List<string>();

        protected int port = 0;
        protected byte error = 0;
        protected string adres = string.Empty;
        protected int connectionID = 0;
        protected int recHostId;
        protected int channelID;
        protected int dataSize;
        protected byte[] recBuffer = null;

        protected PeerInfo newPear = null;

        protected Dictionary<QosType, int> channelDictionary = new Dictionary<QosType, int>();

        [SerializeField]
        protected float _send = 0;
        [SerializeField]
        protected float _received = 0;

        public float Send = 0;
        public float Received = 0;
        public float Total = 0;

        [SerializeField]
        protected float _counter = 0;

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
            int portToUse = _settings.PearType == NetworkManagerTypeEnum.Server ? _settings.Port : 0;
            hostID = NetworkTransport.AddHost(topology, portToUse);

            this.enabled = true;
        }

        public virtual void StartSession()
        {
            _settings.PearType = NetworkManagerTypeEnum.Server;
            if (SaveLoadManager.Instance != null)
            {
                SaveLoadManager.Instance.GameLoadedCallBack -= CreateMatch;
                SaveLoadManager.Instance.GameLoadedCallBack += CreateMatch;
            }
            else
            {
                CreateMatch();
            }
        }

        public virtual void JoinSession()
        {
            _settings.PearType = NetworkManagerTypeEnum.Server;

            networkMatch.ListMatches(0, 1, "", true, 0, 0, (success, info, matches) =>
            {
                if (success && matches.Count > 0)
                {
                    networkMatch.JoinMatch(matches[0].networkId, "", "", "", 0, 0, OnMatchJoined);
                }
            });
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

            // Make sure if connected pears list is empty
            connectedPeers.Clear();
        }

        public virtual void Start() { }

        protected virtual void OnDestroy() { }

        public virtual void SetPeerType(NetworkManagerTypeEnum type)
        {
            _settings.PearType = type;
        }

        protected virtual void OnApplicationQuit()
        {
            NetworkTransport.Shutdown();
        }

        protected virtual void ClientConnected(int connectionId) { }

        protected virtual void HandleConnection()
        {
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

            string log = string.Format(
                NetworkManagerLogs.NEW_CONNECTION_APPEARED,
                NetworkUtility.GetIPAdress(adres),
                port,
                System.DateTime.Now.ToString());

            _logs.Add(log);

            NetworkError networkError = NetworkUtility.GetNetworkError(error);
            if (networkError == NetworkError.Ok)
            {
                newPear = new PeerInfo(connectionID, adres, port);

                log = string.Format(
                    NetworkManagerLogs.CONNECTING_TO_PEER_SUCCEEDED,
                    newPear.IPAdres,
                    newPear.Port,
                    System.DateTime.Now.ToString());

                _logs.Add(log);

                connectedPeers.Add(newPear);

                ClientConnected(newPear.ConnectionID);
            }
        }

        protected virtual void ClientDisconnected(int connectionID) { }

        protected virtual void HandleDisconnection()
        {
            for (int i = 0; i < connectedPeers.Count; i++)
            {
                PeerInfo info = connectedPeers[i];
                if (info.ConnectionID == connectionID)
                {
                    connectedPeers.RemoveAt(i);
                    break;
                }
            }

            ClientDisconnected(connectionID);
        }

        protected virtual void HandleMessages(byte[] buffer, int size) {}

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

        protected virtual void SendToAllReliable(byte[] message, int skipConnectionID = -1)
        {
            for (int i = 0; i < connectedPeers.Count; i++)
            {
                int connectionID = connectedPeers[i].ConnectionID;
                if (skipConnectionID > 0 && connectionID == skipConnectionID)
                {
                    continue;
                }

                SendReliable(message, connectedPeers[i].ConnectionID);
            }
        }

        protected virtual NetworkError SendReliable(byte[] message, int connectionId)
        {
            NetworkTransport.Send(
                hostID,
                connectionId,
                channelDictionary[QosType.Reliable],
                message,
                message.Length,
                out error);

            NetworkError networkError = NetworkUtility.GetNetworkError(error);

            if (networkError == NetworkError.Ok)
            {
                _send += message.Length;
            }

            return networkError;
        }

        protected virtual void UpdateForAllReliable(byte[] message, int messageSize = 0)
        {
            for (int i = 0; i < connectedPeers.Count; i++)
            {
                UpdateUnreiable(message, connectedPeers[i].ConnectionID, messageSize);
            }
        }

        protected virtual NetworkError UpdateUnreiable(byte[] message, int connectionId, int messageSize = 0)
        {
            if(message == null || message.Length == 0)
            {
                return NetworkError.Ok;
            }

            int size = messageSize > 0 ? messageSize : message.Length;
            NetworkTransport.Send(
                hostID,
                connectionId,
                channelDictionary[QosType.UnreliableSequenced],
                message,
                size,
                out error);

            NetworkError networkError = NetworkUtility.GetNetworkError(error);
            if (networkError == NetworkError.Ok)
            {
                _send += size;
            }

            return networkError;
        }

        protected virtual void Update()
        {
            if (hostID == -1)
            {
                this.enabled = false;
                return;
            }

            _counter += Time.deltaTime;

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
                    out channelID,
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
                        _received += dataSize;
                        HandleMessages(recBuffer, dataSize);
                        break;

                    case NetworkEventType.DisconnectEvent:
                        HandleDisconnection();
                        break;

                    case NetworkEventType.Nothing:
                        break;
                }
            }
            while (networkEvent != NetworkEventType.Nothing);

            if (_counter > 1f)
            {
                _counter = 0;
                Send = _send;
                Received = _received;

                Total = Send + Received;

                _send = 0;
                _received = 0;
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

                StartServer(
                    matchInfo.address,
                    matchInfo.port,
                    matchInfo.networkId,
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
                matchList = matches;
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
            _settings.PearType = NetworkManagerTypeEnum.Server;
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

        public virtual void ConnectThroughRelay(string relayIP, int relayPort, NetworkID networkID, NodeID nodeID)
        {
            _settings.PearType = NetworkManagerTypeEnum.Client;
            Initialize();

            SourceID sourceID = Utility.GetSourceID();
            NetworkTransport.ConnectToNetworkPeer(
                hostID,
                relayIP,
                relayPort,
                0,
                0,
                networkID,
                sourceID,
                nodeID,
                out error);
        }

        public byte[] ConvertObjectToBytes(object objectToConvert)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream();

            binaryFormatter.Serialize(memoryStream, objectToConvert);
            byte[] array = memoryStream.ToArray();

            return array;
        }

        public T ConvertBytesToObject<T>(byte[] array, int start = 0, int length = 0)
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            MemoryStream memoryStream = new MemoryStream(array, start, length > 0 ? length - start : array.Length - start);

            T objectFormBytes = (T)binaryFormatter.Deserialize(memoryStream);

            return objectFormBytes;
        }
    }
}
