using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.States
{
	public abstract class BaseStateGraph : MonoBehaviour 
	{
        [SerializeField] private List<Node> _nodeInfo = new List<Node>();
        public List<Node> NodeInfo { get { return _nodeInfo; } }

		[SerializeField] private BaseState _rootState = null;
        public BaseState RootState
        {
            get { return _rootState; }
            set { _rootState = value; }
        }

        [SerializeField] private List<StateTransition> _formEnyStateTransition = new List<StateTransition>();
        public List<StateTransition> FormEnyStateTransition { get { return _formEnyStateTransition; } }


        public void HandleTransitions(BaseStateHandler handler) 
		{
			foreach (var item in _formEnyStateTransition)
			{
				item.Validate(handler);
			}

			foreach (var item in handler.CurrentState.StateTransition)
			{
				item.Validate(handler);
			}
		}
	}
}