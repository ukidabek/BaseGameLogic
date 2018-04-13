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
        private List<Node> _nodes = new List<Node>();
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

        private GenericMenu _addStateContextMenu = new GenericMenu();
        private GenericMenu _addContitionContextMenu = new GenericMenu();

        private GenericMenu _selectedNodeContextMenu = new GenericMenu();
        private GenericMenu _selectedTransitionContextMenu = new GenericMenu();

        private Vector2 _currentMousePossition = Vector2.zero;

        private Connector _connector = null;
        private Connector _formAnyStateConnector = null;

        public StateGraphEditorWindow()
        {
            titleContent = new GUIContent("State Graph");
            minSize = new Vector2(800, 600);
        }

        public void Initialize(BaseStateGraph stateGraph)
        {
            _stateGraph = stateGraph;

            _connector = new Connector(SetDirty);
            _formAnyStateConnector = new Connector(SetDirty, _stateGraph);

            _nodes.AddRange(_stateGraph.NodeInfo);
            _nodes.Add(_stateGraph.FromAnyStateNode);

            _stateGraph.FromAnyStateNode.OnConnectionPointClicked += _formAnyStateConnector.Connect;

            foreach (var item in _nodes)
            {
                if (item.State != null) 
                    _statesDictionary.Add(item.State, item);

                item.IsSelected = false;
                item.OnConnectionPointClicked += _formAnyStateConnector.Connect;
                item.OnConnectionPointClicked += _connector.Connect;
            }
        }

        private void Awake()
        {
            _menuAreaRect.position = Vector2.zero;
            _menuAreaRect.size = new Vector2(position.width, EditorGUIUtility.singleLineHeight);

            CalculateInspectorAndGraphRect();

            _addStateContextMenu = GenerateAddMenu(AssemblyExtension.GetDerivedTypes<BaseState>(), "Add state/{0}", AddState);
            _addContitionContextMenu = GenerateAddMenu(AssemblyExtension.GetDerivedTypes<BaseStateTransitionCondition>(), "{0}", AddCondition);

            _selectedNodeContextMenu.AddItem(new GUIContent("Remove state"), false, RemoveState);
            _selectedTransitionContextMenu.AddItem(new GUIContent("Remove transition"), false, RemoveTransition);
        }

        private GenericMenu GenerateAddMenu(Type[] typesToAdd, string formatString, GenericMenu.MenuFunction2 addState, GenericMenu existingMenu = null)
        {
            GenericMenu menu = existingMenu != null ? existingMenu : new GenericMenu();
            foreach (var item in typesToAdd)
            {
                GUIContent content = new GUIContent(string.Format(formatString, item.Name));
                menu.AddItem(content, false, addState, item);
            }

            return menu;
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
            _stateGraph.FromAnyStateNode.OnConnectionPointClicked -= _formAnyStateConnector.Connect;

            foreach (var item in _nodes)
            {
                item.OnConnectionPointClicked -= _connector.Connect;
                item.OnConnectionPointClicked -= _formAnyStateConnector.Connect;
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
            foreach (var item in _nodes)
            {
                if (item.ProcessEvents(current, new Vector2(0, _menuAreaRect.height)))
                {
                    GUI.changed = true;
                }

                if (item.IsSelected)
                {
                    _selectedNode = item;
                    _selectedTransition = null;
                }
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
                            if(_selectedNode != null)
                            {
                                _selectedNode.IsSelected = false;
                                _selectedNode = null;
                            }
                            GUI.changed = true;
                            break;
                        }
                    }
                    switch(current.button)
                    {
                        case 1:
                            if (_selectedNode == null)
                            {
                                if(transitionSelected)
                                    _selectedTransitionContextMenu.ShowAsContext();
                                else
                                    _addStateContextMenu.ShowAsContext();
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

                for (int i = 0; i < _stateGraph.FormAnyStateTransition.Count; i++)
                {
                    DrawTransitionBezier(
                        _stateGraph.FromAnyStateNode,
                        _stateGraph.FormAnyStateTransition[i],
                        -1,
                        i);
                }

                for (int i = 0; i < _nodes.Count; i++)
                {
                    var node = _nodes[i];
                    if (node.State == null) continue;
                    for (int j = 0; j < node.State.Transitions.Count; j++)
                    {
                        var transition = node.State.Transitions[j];

                        if (transition.TargetState == null)
                        {
                            node.State.Transitions.RemoveAt(j--);
                        }
                        else
                        {
                            DrawTransitionBezier(node, transition, i, j);
                        }
                    }
                    node.Draw();
                }
                _stateGraph.FromAnyStateNode.Draw();
            }
            GUILayout.EndArea();
        }

        private void DrawTransitionBezier(Node node, StateTransition transition, int i, int j)
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

        private void DrawInspectorArea()
        {
            GUILayout.BeginArea(_inspectorAreaRecr);
            {
                DrawTransitionInspector();
                DrawStateInspecotr();
            }
            GUILayout.EndArea();
        }

        private void DrawStateInspecotr()
        {
            if (_selectedNode != null && _selectedNode.State != null)
            {
                DrawInspectorArea(_selectedNode.State);

                EditorGUILayout.Space();

                DrawTransitionConditionsInspector(_selectedNode.State.ExitStateConditons);
            }
        }

        private void DrawTransitionInspector()
        {
            if (_selectedTransition != null)
            {
                GUIStyle style = new GUIStyle();
                style.richText = true;
                EditorGUILayout.LabelField("<b>Transition conditions</b>", style);
                StateTransition transition = _stateGraph[_selectedTransition.NodeIndex, _selectedTransition.TransitionIndex];

                DrawTransitionConditionsInspector(transition.Conditions);
            }
        }

        private void DrawTransitionConditionsInspector(List<BaseStateTransitionCondition> conditions)
        {
            for (int i = 0; i < conditions.Count; i++)
            {
                var item = conditions[i];
                if (item == null)
                {
                    conditions.RemoveAt(i);
                    --i;
                    continue;
                }

                DrawInspectorArea(item);
            }

            EditorGUILayout.Space();

            if (GUILayout.Button("Add condition"))
                _addContitionContextMenu.ShowAsContext();
        }

        private void DrawInspectorArea(MonoBehaviour item)
        {
            if (item == null) return;

            Editor editor = Editor.CreateEditor(item);
            EditorGUILayout.InspectorTitlebar(false, item);
            editor.OnInspectorGUI();
        }

        private void AddState(object data)
        {
            Type type = data as Type;
            BaseState state = _stateGraph.gameObject.AddComponent(type) as BaseState;

            Undo.RecordObject(_stateGraph, "State added");

            Node newNode = new Node(_currentMousePossition, state);
            _statesDictionary.Add(state, newNode);

            newNode.OnConnectionPointClicked += _formAnyStateConnector.Connect;
            newNode.OnConnectionPointClicked += _connector.Connect;

            _stateGraph.NodeInfo.Add(newNode);
            _nodes.Add(newNode);

            SetDirty(_stateGraph);

            if (_stateGraph.RootState == null)
                _stateGraph.RootState = state;
        }

        private void AddCondition(object data)
        {
            if(_selectedTransition != null)
            {
                Type type = data as Type;
                var condition = _stateGraph.gameObject.AddComponent(type) as BaseStateTransitionCondition;

                Undo.RecordObject(_stateGraph, "Connection added");

                var transition = _stateGraph[_selectedTransition.NodeIndex, _selectedTransition.TransitionIndex];
                transition.Conditions.Add(condition);
            }
        }

        private void RemoveState()
        {
            if (_selectedNode != null && _selectedNode.State != null)
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
                if(_selectedTransition.NodeIndex < 0)
                {
                    RemoveAllContitionObjects(_stateGraph.FormAnyStateTransition[_selectedTransition.TransitionIndex]);
                    _stateGraph.FormAnyStateTransition.RemoveAt(_selectedTransition.TransitionIndex);
                }
                else
                {
                    RemoveAllContitionObjects(_stateGraph[_selectedTransition.NodeIndex, _selectedTransition.TransitionIndex]);
                    _stateGraph.NodeInfo[_selectedTransition.NodeIndex].State.Transitions.RemoveAt(_selectedTransition.TransitionIndex);
                }

                _selectedTransition = null;
                SetDirty(_stateGraph);
            }
        }

        private void RemoveAllContitionObjects(StateTransition stateTransition)
        {
            for (int i = 0; i < stateTransition.Conditions.Count; i++)
            {
                DestroyImmediate(stateTransition.Conditions[i]);
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
}