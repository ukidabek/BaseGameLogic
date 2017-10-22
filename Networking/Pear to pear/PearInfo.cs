using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking.PearToPear
{
    [Serializable]
    public class PearInfo
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

        public PearInfo(string ipAdress, int port)
        {
            char[] separators = { ':' };
            string[] ipAdresParts = ipAdress.Split(separators);
            _ipAdres = ipAdresParts[ipAdresParts.Length - 1];
            _port = port;
        } 
    }
}