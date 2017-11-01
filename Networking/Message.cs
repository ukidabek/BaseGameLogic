using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking
{
    [Serializable]
    public class Message
    {
        private int _messageID = -1;
        public int MessageID
        {
            get { return _messageID; }
        }

        public object Data = null;

        private Message() {}

        public Message(int messafgeID)
        {
            _messageID = messafgeID;
        }
    }
}