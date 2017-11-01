using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking.PearToPear
{
    [Serializable]
    public class PeerToPeerMessage
    {
        private int _messageID = -1;
        public int MessageID
        {
            get { return _messageID; }
        }

        public object Data = null;

        private PeerToPeerMessage() {}

        public PeerToPeerMessage(int messafgeID)
        {
            _messageID = messafgeID;
        }
    }
}