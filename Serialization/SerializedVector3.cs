using UnityEngine;

using System;

namespace BaseGameLogic.Serialization
{
    [Serializable]
    public class SerializedVector3 : SerializedVector2
    {
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

        public SerializedVector3(): base () {}

        public SerializedVector3(Vector3 vector3) : base (vector3)
        {
            Z = vector3.z;
        }
    }
}