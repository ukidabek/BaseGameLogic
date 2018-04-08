using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.States
{
    [Serializable]
    public class StateTransition
    { 
        [SerializeField] private BaseState _targetState = null;
        public BaseState TargetState { get { return _targetState; } }

        [SerializeField] private List<BaseStateTransitionCondition> _conditions = new List<BaseStateTransitionCondition>();


        public void Validate(BaseStateHandler stateHandler)
        {
            for (int i = 0; i < _conditions.Capacity; i++)
            {
                if (!_conditions[i].Validate() || _targetState == stateHandler.CurrentState)
                    return;
            }

            stateHandler.EnterState(_targetState);
        }
    }
}