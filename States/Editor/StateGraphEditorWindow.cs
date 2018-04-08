using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.States
{

    public class StateGraphEditorWindow : EditorWindow
    {
        private BaseStateGraph _stateGraph = null;

        private Dictionary<BaseState, Node> _statesDictionary = new Dictionary<BaseState, Node>();
        private Node _selectedNode = null;

        private Vector2 offset;
        private Vector2 drag;

        private Rect _graphAreaRect = new Rect();
        private Rect _menuAreaRect = new Rect();

        private GenericMenu _contextMenu = new GenericMenu();

        private Vector2 _currentMousePossition = Vector2.zero;

        public StateGraphEditorWindow(BaseStateGraph stateGraph)
        {
            _stateGraph = stateGraph;
            minSize = new Vector2(800, 600);

            foreach (var item in _stateGraph.NodeInfo)
            {
                _statesDictionary.Add(item.State, item);
            }

            Show();
        }

        private void Awake()
        {
            _menuAreaRect.position = Vector2.zero;
            _menuAreaRect.size = new Vector2(position.width, EditorGUIUtility.singleLineHeight);

            _graphAreaRect = position;

            _graphAreaRect.x = 0;
            _graphAreaRect.y = EditorGUIUtility.singleLineHeight;

            Type[] stateTypes = AssemblyExtension.GetDerivedTypes<BaseState>();

            foreach (var item in stateTypes)
            {
                GUIContent content = new GUIContent(string.Format("Add state/{0}", item.Name));
                _contextMenu.AddItem(content, false, AddState, item);
            }
        }

        private void OnGUI()
        {
            _graphAreaRect.size = position.size;
            DrawMenuArea();
            DrawGraphArea();

            ProcessNodeEvents(Event.current);
            ProcessEvents(Event.current);

            if (GUI.changed) Repaint();
        }

        private void ProcessNodeEvents(Event current)
        {
            _selectedNode = null;
            foreach (var item in _stateGraph.NodeInfo)
            {
                if (item.ProcessEvents(current, new Vector2(0, _menuAreaRect.height)))
                {
                    GUI.changed = true;
                }

                if (item.IsSelected) _selectedNode = item;
            }
        }

        private void ProcessEvents(Event current)
        {
            switch (current.type)
            {
                case EventType.MouseDown:
                    if (current.button == 1)
                    {
                        _contextMenu.ShowAsContext();
                        _currentMousePossition = current.mousePosition;
                    }
                    break;

            }
        }

        private void DrawMenuArea()
        {
            GUILayout.BeginArea(_menuAreaRect);
            {
            }
            GUILayout.EndArea();
        }

        private void DrawGraphArea()
        {
            GUILayout.BeginArea(_graphAreaRect);
            {
                DrawGrid(position, 10, 0.2f, Color.gray);
                DrawGrid(position, 100, 0.4f, Color.gray);

                foreach (var node in _stateGraph.NodeInfo)
                {
                    node.Draw();
                }
            }
            GUILayout.EndArea();
        }

        private void AddState(object data)
        {
            Type type = data as Type;
            BaseState state = _stateGraph.gameObject.AddComponent(type) as BaseState;

            Node newNode = new Node(_currentMousePossition, state);
            _stateGraph.NodeInfo.Add(newNode);

            if (_stateGraph.RootState == null)
                _stateGraph.RootState = state;
        }

        private void DrawGrid(Rect rect, float gridSpacing, float gridOpacity, Color gridColor)
        {
            int widthDivs = Mathf.CeilToInt(rect.width / gridSpacing);
            int heightDivs = Mathf.CeilToInt(rect.height / gridSpacing);

            Handles.BeginGUI();
            {
                Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

                offset += drag * 0.5f;
                Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

                for (int i = 0; i < widthDivs; i++)
                {
                    Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
                }

                for (int j = 0; j < heightDivs; j++)
                {
                    Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
                }

                Handles.color = Color.white;
            }
            Handles.EndGUI();
        }
    }

    //public class Transition
}