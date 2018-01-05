using UnityEngine;

using System;

namespace BaseGameLogic.Serialization
{
    [Serializable]
    public class SerializedVector3
    {
        public float X = 0;
        public float Y = 0;
        public float Z = 0f;

        public Vector3 Vector3
        {
            get { return new Vector3(X, Y, Z); }
            set
            {
                X = value.x;
                Y = value.y;
                Z = value.z;
            }
        }

        public SerializedVector3() {}

        public SerializedVector3(Vector3 vector3)
        {
            X = vector3.x;
            Y = vector3.y;
            Z = vector3.z;
        }
    }
}