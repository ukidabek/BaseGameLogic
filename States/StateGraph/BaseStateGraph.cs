using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using System;

namespace BaseGameLogic.States
{
    public abstract class BaseStateGraph : MonoBehaviour
    {
        [SerializeField] private Node _fromAnyStateNode = new Node();
        public Node FromAnyStateNode { get { return _fromAnyStateNode; } }

        [SerializeField] private List<Node> _nodeInfo = new List<Node>();
        public List<Node> NodeInfo { get { return _nodeInfo; } }

        [SerializeField] private List<StateTransition> _formAnyStateTransition = new List<StateTransition>();
        public List<StateTransition> FormAnyStateTransition { get { return _formAnyStateTransition; } }

        [SerializeField] private BaseState _rootState = null;
        public BaseState RootState
        {
            get { return _rootState; }
            set { _rootState = value; }
        }


        public StateTransition this[int i, int y]
        {
            get
            {
                if(i < 0)
                {
                    return _formAnyStateTransition[y];
                }

                return _nodeInfo[i].State.Transitions[y];
            }
        }

        public void HandleTransitions(BaseStateHandler handler) 
		{
            HandleTransitionLoop(handler, _formAnyStateTransition);
            HandleTransitionLoop(handler, handler.CurrentState.Transitions);

            bool exitState = false;
            foreach (var item in handler.CurrentState.ExitStateConditons)
            {
                exitState = item.Validate();
                if (!exitState) break;
            }
            if (exitState) handler.ExitState();
		}

        private void HandleTransitionLoop(BaseStateHandler handler, List<StateTransition> transitions)
        {
            foreach (var item in transitions)
            {
                item.Validate(handler);
            }
        }
    }
}