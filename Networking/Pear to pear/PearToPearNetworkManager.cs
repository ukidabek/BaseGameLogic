using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Types;

using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace BaseGameLogic.Networking.PearToPear
{
    public abstract class PearToPearNetworkManager : MonoBehaviour
    {
        [SerializeField]
        private int port = 8888;

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

            int portToUse = _pearType == PearToPearNetworkManagerEnum.MasterPear ? port : 0;
            hostID = NetworkTransport.AddHost(topology, portToUse);
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
                    int port;
                    string adres = string.Empty;
                    NetworkID networkID;
                    NodeID node;
                    NetworkTransport.GetConnectionInfo(hostID, connectionId, out adres, out port, out networkID, out node, out error);
                    Debug.Log(adres);
                    Debug.Log(port);
                    PearInfo info = new PearInfo(adres, port);
                    PearToPearMessage message = new PearToPearMessage(PearToPearMessageID.NEW_PEAR);
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
                        PearToPearMessage m = bff.Deserialize(ms) as PearToPearMessage;
                        size = ms.ToArray().Length;
                        recBuffer = ms.ToArray();

                        if(m.MessageID == PearToPearMessageID.NEW_PEAR)
                        {
                            PearInfo infoo = m.Data as PearInfo;
                        }
                    }

                    break;
                case NetworkEventType.DisconnectEvent: //4
                    break;
            }
        }

        public void Connect()
        {
            int connectionId;
            byte error;
            connectionId = NetworkTransport.Connect(hostID, "127.0.0.1", port, 0, out error);
        }
    }
}
