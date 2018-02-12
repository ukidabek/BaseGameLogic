using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Networking
{
    public abstract class BaseNetworkUpdater : MonoBehaviour
    {
        public abstract int MessageID { get; }
        protected abstract void Update();
    }
}