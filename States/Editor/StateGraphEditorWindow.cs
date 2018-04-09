using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;

namespace BaseGameLogic.States
{
    public class StateGraphEditorWindow : EditorWindow
    {
        private BaseStateGraph _stateGraph = null;
        private List<TransitionInfo> _transitionRectList = new List<TransitionInfo>();
        private TransitionInfo _selectedTransition = null;

        private Dictionary<BaseState, Node> _statesDictionary = new Dictionary<BaseState, Node>();
        private Node _selectedNode = null;

        private Vector2 offset;
        private Vector2 drag;

        private Rect _graphAreaRect = new Rect();
        private Rect _menuAreaRect = new Rect();
        private Rect _inspectorAreaRecr = new Rect();
        private float _inspectorSize = .3f;

        private GenericMenu _contextMenu = new GenericMenu();
        private GenericMenu _selectedNodeContextMenu = new GenericMenu();
        private GenericMenu _selectedTransitionContextMenu = new GenericMenu();


        private Vector2 _currentMousePossition = Vector2.zero;

        private Connector _connector = null;

        public StateGraphEditorWindow()
        {
            titleContent = new GUIContent("State Graph");
            minSize = new Vector2(800, 600);
        }

        public void Initialize(BaseStateGraph stateGraph)
        {
            _stateGraph = stateGraph;

            _connector = new Connector(SetDirty);

            foreach (var item in _stateGraph.NodeInfo)
            {
                _statesDictionary.Add(item.State, item);
                item.OnConnectionPointClicked += _connector.Connect;
            }
        }

        private void Awake()
        {
            _menuAreaRect.position = Vector2.zero;
            _menuAreaRect.size = new Vector2(position.width, EditorGUIUtility.singleLineHeight);

            CalculateInspectorAndGraphRect();

            Type[] stateTypes = AssemblyExtension.GetDerivedTypes<BaseState>();

            foreach (var item in stateTypes)
            {
                GUIContent content = new GUIContent(string.Format("Add state/{0}", item.Name));
                _contextMenu.AddItem(content, false, AddState, item);
            }

            _selectedNodeContextMenu.AddItem(new GUIContent("Remove state"), false, RemoveState);
            _selectedTransitionContextMenu.AddItem(new GUIContent("Remove transition"), false, RemoveTransition);
        }

        private void CalculateInspectorAndGraphRect()
        {
            _inspectorAreaRecr.position = new Vector2(position.width * (1 - _inspectorSize), _menuAreaRect.height);
            _inspectorAreaRecr.size = new Vector2(position.width * _inspectorSize, position.height - _menuAreaRect.height);


            _graphAreaRect.position = new Vector2(0, _menuAreaRect.height);
            _graphAreaRect.size = new Vector2(position.width * (1 - _inspectorSize), position.height - _menuAreaRect.height);
        }

        private void OnDestroy()
        {
            foreach (var item in _stateGraph.NodeInfo)
            {
                item.OnConnectionPointClicked -= _connector.Connect;
            }
        }

        private void OnGUI()
        {
            CalculateInspectorAndGraphRect();
            DrawMenuArea();
            DrawGraphArea();
            DrawInspectorArea();

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

                if (item.IsSelected)
                    _selectedNode = item;
            }
        }

        private void ProcessEvents(Event current)
        {
            switch (current.type)
            {
                case EventType.MouseDown:
                    bool transitionSelected = false;
                    foreach (var item in _transitionRectList)
                    {
                        if (item.Rect.Contains(current.mousePosition - new Vector2(0, _menuAreaRect.height)))
                        {
                            transitionSelected = true;
                            _selectedTransition = item;
                            break;
                        }
                    }
                    switch(current.button)
                    {
                        case 1:
                            if (_selectedNode == null)
                            {
                                //_selectedTransition = null;

                                if(transitionSelected)
                                    _selectedTransitionContextMenu.ShowAsContext();
                                else
                                    _contextMenu.ShowAsContext();
                            }
                            else
                            {
                                _selectedNodeContextMenu.ShowAsContext();
                            }
                            break;
                    }
                       
                    _currentMousePossition = current.mousePosition;
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

                _transitionRectList.Clear();

                for (int i = 0; i < _stateGraph.NodeInfo.Count; i++)
                {
                    var node = _stateGraph.NodeInfo[i];
                    for (int j = 0; j < node.State.Transitions.Count; j++)
                    {
                        var transition = node.State.Transitions[j];

                        if (transition.TargetState == null)
                        {
                            node.State.Transitions.RemoveAt(j--);
                        }
                        else
                        {
                            Node targetNode = _statesDictionary[transition.TargetState];

                            Handles.DrawBezier(
                                node.Out.Rect.center,
                                targetNode.In.Rect.center,
                                node.Out.Rect.center - Vector2.left * 50f,
                                targetNode.In.Rect.center + Vector2.left * 50f,
                                Color.white,
                                null,
                                2f);

                            Rect transitionRect = new Rect(Vector2.zero, new Vector2(10, 10));
                            transitionRect.center = ((node.Out.Rect.center + targetNode.In.Rect.center) / 2);
                            _transitionRectList.Add(new TransitionInfo(transitionRect, i, j));
                            GUI.Box(transitionRect, "");
                        }
                    }
                    node.Draw();
                }
            }
            GUILayout.EndArea();
        }

        private void DrawInspectorArea()
        {
            GUILayout.BeginArea(_inspectorAreaRecr);
            {
                if(_selectedTransition != null)
                {
                    StateTransition transition = _stateGraph.NodeInfo[_selectedTransition.NodeIndex].State.Transitions[_selectedTransition.TransitionIndex];
                    foreach (var item in transition.Conditions)
                    {
                        Editor editor = Editor.CreateEditor(item);
                        EditorGUILayout.InspectorTitlebar(false, item);
                        editor.DrawDefaultInspector();
                    }
                }
            }
            GUILayout.EndArea();
        }

        private void AddState(object data)
        {
            Type type = data as Type;
            BaseState state = _stateGraph.gameObject.AddComponent(type) as BaseState;

            Undo.RecordObject(_stateGraph, "State added");

            Node newNode = new Node(_currentMousePossition, state);
            _stateGraph.NodeInfo.Add(newNode);

            SetDirty(_stateGraph);

            if (_stateGraph.RootState == null)
                _stateGraph.RootState = state;
        }

        private void RemoveState()
        {
            if (_selectedNode != null)
            {
                Undo.RecordObject(_stateGraph.gameObject, "State removed");
                int index = _stateGraph.NodeInfo.IndexOf(_selectedNode);
                _stateGraph.NodeInfo.RemoveAt(index);
                _selectedNode.Remove();
                _selectedNode = null;
                SetDirty(_stateGraph);
            }
        }

        private void RemoveTransition()
        {
            if (_selectedTransition != null)
            {
                Undo.RecordObject(_stateGraph, "Transition removed");
                _stateGraph.NodeInfo[_selectedTransition.NodeIndex].State.Transitions.RemoveAt(_selectedTransition.TransitionIndex);
                _selectedTransition = null;
                SetDirty(_stateGraph);
            }
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

        private void SetDirty(MonoBehaviour monoBehaviour)
        {
            if (monoBehaviour == null) return;

            EditorUtility.SetDirty(monoBehaviour);
            EditorSceneManager.MarkSceneDirty(monoBehaviour.gameObject.scene);
        }
    }

    internal class Connector
    {
        private BaseState _inNode = null;
        private BaseState _outNode = null;

        private Action<MonoBehaviour> SetDirty = null;

        public Connector(Action<MonoBehaviour> setDirty)
        {
            SetDirty = setDirty;
        }

        public void Connect(ConnectionPointType type, BaseState state)
        {
            switch (type)
            {
                case ConnectionPointType.In:
                    if (_inNode == null) _inNode = state;
                    break;
                case ConnectionPointType.Out:
                    if (_outNode == null) _outNode = state;
                    break;
            }

            if(_inNode != null && _outNode != null)
            {
                Undo.RecordObject(_outNode.gameObject, "Transition added");
                _outNode.Transitions.Add(new StateTransition(_inNode));
                _inNode = _outNode = null;

                if(SetDirty != null)
                {
                    SetDirty(_outNode);
                    SetDirty(_inNode);
                }
            }
        }
    }

    internal class TransitionInfo
    {
        public Rect Rect;
        public int NodeIndex = 0;
        public int TransitionIndex = 0;

        public TransitionInfo(Rect rect, int nodeInde, int transitionIndex)
        {
            Rect = rect;
            NodeIndex = nodeInde;
            TransitionIndex = transitionIndex;
        }
    }
}