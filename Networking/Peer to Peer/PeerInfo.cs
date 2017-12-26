using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking.PeerToPeer
{
    [Serializable]
    public class PeerInfo
    {
        [SerializeField]
        private int _port = 0;
        public int Port
        {
            get { return _port; }
        }

        [SerializeField]
        public string _ipAdres = string.Empty;
        public string IPAdres
        {
            get { return _ipAdres; }
        }

        public int _connectionID = -1;
        public int ConnectionID
        {
            get { return _connectionID; }
            set { _connectionID = value; }
        }

        public PeerInfo(string ipAdress, int port)
        {
            _ipAdres = NetworkUtility.GetIPAdress(ipAdress);
            _port = port;
        } 
    }
}