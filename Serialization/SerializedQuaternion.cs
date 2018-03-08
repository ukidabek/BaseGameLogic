using UnityEngine;

using System;

namespace BaseGameLogic.Serialization
{
    [Serializable]
    public class SerializedQuaternion
    {
        public float W = 0;
        public float X = 0;
        public float Y = 0;
        public float Z = 0;

        public Quaternion Quaternion
        {
            get { return new Quaternion(X, Y, Z, W); }
            set
            {
                W = value.w;
                X = value.x;
                Y = value.y;
                Z = value.z;
            }
        }

        public SerializedQuaternion() {}

        public SerializedQuaternion(Quaternion quaternion)
        {
            W = quaternion.w;
            X = quaternion.x;
            Y = quaternion.y;
            Z = quaternion.z;
        }
    }
}