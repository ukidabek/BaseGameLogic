using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking.PeerToPeer
{
    [Serializable]
    public class PeerInfo
    {
        private int _port = 0;
        public int Port
        {
            get { return _port; }
        }

        public string _ipAdres = string.Empty;
        public string IPAdres
        {
            get { return _ipAdres; }
        }

        public PeerInfo(string ipAdress, int port)
        {
            char[] separators = { ':' };
            string[] ipAdresParts = ipAdress.Split(separators);
            _ipAdres = ipAdresParts[ipAdresParts.Length - 1];
            _port = port;
        } 
    }
}