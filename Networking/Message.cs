using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking
{
    [Serializable]
    public class Message
    {
        private byte _messageID = 1;
        public byte MessageID
        {
            get { return _messageID; }
        }

        public object Data = null;

        private Message() {}

        public Message(byte messafgeID)
        {
            _messageID = messafgeID;
        }
    }
}