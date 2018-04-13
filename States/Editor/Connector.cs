using UnityEngine;
using UnityEditor;

using System;

namespace BaseGameLogic.States
{
    internal class Connector 
    {
        private BaseState _inNode = null;
        private BaseState _outNode = null;

        private Action<MonoBehaviour> SetDirty = null;

        private BaseStateGraph _graph = null;
        private bool _formAnyStateTransition = false;
        private Node _node = null;

        public Connector(Action<MonoBehaviour> setDirty)
        {
            SetDirty = setDirty;
        }

        public Connector(Action<MonoBehaviour> setDirty, BaseStateGraph graph) : this(setDirty)
        {
            _node = graph.FromAnyStateNode;

            SetDirty = setDirty;
            _graph = graph;
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
                    if (_graph != null) _formAnyStateTransition = true;
                    break;
            }

            if(_inNode != null && _outNode != null)
            {
                Undo.RecordObject(_outNode.gameObject, "Transition added");
                _outNode.Transitions.Add(new StateTransition(_inNode));

                if(SetDirty != null)
                {
                    SetDirty(_outNode);
                    SetDirty(_inNode);
                }

                _inNode = _outNode = null;
            }

            if(_graph != null && _formAnyStateTransition && _inNode != null)
            {
                Undo.RecordObject(_inNode.gameObject, "From Any state transition added");

                _graph.FormAnyStateTransition.Add(new StateTransition(_inNode));

                if (SetDirty != null)
                {
                    SetDirty(_graph);
                    SetDirty(_outNode);
                }

                _formAnyStateTransition = false;
                _inNode = null;
            }
        }
    }
}