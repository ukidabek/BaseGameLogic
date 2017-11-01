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
        private int port = 8888;

        [SerializeField]
        protected int connectionsCount = 8;
        
        [SerializeField]
        protected PeerToPeerNetworkManagerEnum _pearType = PeerToPeerNetworkManagerEnum.MasterPear;

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

            int portToUse = _pearType == PeerToPeerNetworkManagerEnum.MasterPear ? port : 0;
            hostID = NetworkTransport.AddHost(topology, portToUse);

            byte error = 0;
            NetworkTransport.SetBroadcastCredentials(hostID, 1, 1, 1, out error);
        }

        protected void AddChanel(ref ConnectionConfig conectionConfig, QosType type)
        {
            int channelId = conectionConfig.AddChannel(type);
            channelDictionary.Add(type, channelId);
        }

        protected virtual void Update()
        {
            //if (_pearType == PearToPearNetworkManagerEnum.Pear)
            //    return;

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
					if (connectionIdTT == connectionId)
						break;
                    int port = 1234;
                    string adres = string.Empty;
                    NetworkID networkID;
                    NodeID node;
                    NetworkTransport.GetConnectionInfo(hostID, connectionId, out adres, out port, out networkID, out node, out error);
                    PeerInfo info = new PeerInfo(adres, port);
                    Message message = new Message(PearToPearMessageID.NEW_PEAR);
                    message.Data = info;
                    BinaryFormatter bf = new BinaryFormatter();
                    int size;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bf.Serialize(ms, message);
                        size = ms.ToArray().Length;
                        recBuffer = ms.ToArray();
                    }
                    int chid = channelDictionary[QosType.Reliable];
                    NetworkTransport.Send(hostID, connectionId, chid, recBuffer, size, out error);
                    break;

                case NetworkEventType.DataEvent:       //3
                    Debug.Log(dataSize);

                    BinaryFormatter bff = new BinaryFormatter();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        ms.Write(recBuffer, 0, dataSize);
                        ms.Seek(0, SeekOrigin.Begin);
                        Message m = bff.Deserialize(ms) as Message;
                        size = ms.ToArray().Length;
                        recBuffer = ms.ToArray();

                        if(m.MessageID == PearToPearMessageID.NEW_PEAR)
                        {
                            PeerInfo infoo = m.Data as PeerInfo;
							Debug.Log(infoo.IPAdres);
                            Debug.Log(infoo.Port);
                        }
                    }

                    break;
                case NetworkEventType.DisconnectEvent: //4
                    break;

                case NetworkEventType.BroadcastEvent:
                    if(_pearType == PeerToPeerNetworkManagerEnum.Pear)
                    {
                        break;
                    }
                    string adress = string.Empty;
                    int p = 0;
                    NetworkTransport.GetBroadcastConnectionInfo(hostID, out adress, out p, out error);
                    Debug.Log(adress + " " + p);
                    char[] s = { ':'};
                    adress = adress.Split(s)[3];
                    connectionIdTT = NetworkTransport.Connect(hostID, adress, p, 0, out error);
                    NetworkError er = (NetworkError)error;
                    Debug.Log(er);
                    break;
            }
        }
		int connectionIdTT;
        public void Connect()
        {
            byte error;

            NetworkTransport.StartBroadcastDiscovery(hostID, port, 1, 1, 1, null, 0, 3, out error);
        }
    }
}
