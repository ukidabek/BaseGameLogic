using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking.PearToPear
{
    [Serializable]
    public class PearToPearMessage
    {
        private int _messageID = -1;
        public int MessageID
        {
            get { return _messageID; }
        }

        public object Data = null;

        private PearToPearMessage() {}

        public PearToPearMessage(int messafgeID)
        {
            _messageID = messafgeID;
        }
    }
}