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

        public bool IsDragged;
        public bool IsSelected;

        public ConnectionPoint In = null;
        public ConnectionPoint Out = null;

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
            GUI.Box(Rect, State.GetType().Name + (IsSelected ? "*" : ""));
            In.Draw();
            Out.Draw();
        }

        public bool ProcessEvents(Event e, Vector2 offset)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    IsSelected = false;
                    if (e.button == 0)
                    {
                        if (Rect.Contains(e.mousePosition - offset))
                        {
                            IsSelected = IsDragged = true;
                            GUI.changed = true;
                        }
                        else
                        {
                            GUI.changed = true;
                        }
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
    }
}