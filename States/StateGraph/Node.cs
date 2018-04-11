using UnityEngine;

using System;

namespace BaseGameLogic.States
{
    [Serializable]
    public class Node
    {
        public Rect Rect = new Rect(Vector2.zero, new Vector2(120, 30));
        public BaseState State = null;
        public Vector2 MenuAreaOffset = Vector2.zero;

        public bool IsDragged = false;
        public bool IsSelected = false;
        public bool RemoveNode = false;

        public ConnectionPoint In = null;
        public ConnectionPoint Out = null;

        public event Action<ConnectionPointType, BaseState> OnConnectionPointClicked = null;

        public Node() {}

        public Node(Vector2 position, BaseState state)
        {
            Rect.position = position;
            State = state;

            Vector2 pointPosition = new Vector2(Rect.x, Rect.y + Rect.height / 2);
            In = new ConnectionPoint(pointPosition);
            pointPosition = new Vector2(Rect.x + Rect.width, Rect.y + Rect.height/2);
            Out = new ConnectionPoint(pointPosition);
        }

        public void Draw()
        {
            GUI.Box(Rect, State != null ? State.GetType().Name : "Any state" + (IsSelected ? "*" : ""));

            if(In.Draw() && OnConnectionPointClicked != null)
            {
                OnConnectionPointClicked(ConnectionPointType.In, State);
            }

            if (Out.Draw() && OnConnectionPointClicked != null)
            {
                OnConnectionPointClicked(ConnectionPointType.Out, State);
            }
        }

        public bool ProcessEvents(Event e, Vector2 offset)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    bool contains = Rect.Contains(e.mousePosition - offset);
                    IsSelected = false;
                    switch(e.button)
                    {
                        case 0:
                        case 1:
                            if (contains)
                            {
                                IsSelected = IsDragged = true;
                                return true;
                            }
                            break;
                    }
                    break;

                case EventType.MouseUp:
                    IsDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && IsDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }

            return false;
        }

        private void Drag(Vector2 delta)
        {
            Rect.position += delta;
            In.Rect.position += delta;
            Out.Rect.position += delta;
        }

        public void Remove()
        {
            GameObject.DestroyImmediate(State);
            RemoveNode = true;
        }
    }
}