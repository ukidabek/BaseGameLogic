using UnityEngine;

using System;

namespace BaseGameLogic.States
{
    [Serializable]
    public partial class ConnectionPoint
    {
        public ConnectionPointType Type;

        public Rect Rect = new Rect();
        public Node Node = null;

        public event Action<ConnectionPoint> OnClic = null;

        public ConnectionPoint()
        {
            Rect.size = new Vector2(10, 10);
        }

        public ConnectionPoint(Vector2 postion, Node node, ConnectionPointType type)
        {
            Rect.size = new Vector2(10, 10);
            Rect.center = postion;
            Node = node;
            this.Type = type;
        }

        public void Draw()
        {
            GUI.Button(Rect, "");
            {
                if (OnClic != null)
                    OnClic(this);
            }
        }
    }
}