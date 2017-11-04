using UnityEngine;

using System;

namespace BaseGameLogic.Serialization
{
    [Serializable]
    public class SerializedVector2
    {
        public float X = 0;
        public float Y = 0;

        public Vector2 Vector2
        {
            get { return new Vector2(X, Y); }
            set
            {
                X = value.x;
                Y = value.y;
            }
        }

        public SerializedVector2() {}

        public SerializedVector2(Vector2 vector)
        {
            X = vector.x;
            Y = vector.y;
        }
    }
}
