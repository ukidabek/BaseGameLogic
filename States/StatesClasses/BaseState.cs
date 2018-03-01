using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Inputs;
using BaseGameLogic.Audio;

namespace BaseGameLogic.States
{
    /// <summary>
    /// Base state.
    /// </summary>
    public abstract class BaseState
    {
		protected List<BaseStateMode> stateModes = new List<BaseStateMode> ();
		public List<BaseStateMode> StateModes
        {
    		get { return this.stateModes; }
    	}

        protected BaseStateObject controlledObject = null;
		public BaseStateObject ControlledObject
        {
    		get { return this.controlledObject; }
    	}

        public BaseState(BaseStateObject controlledObject)
        {
            this.controlledObject = controlledObject;
        }

        public virtual bool EnterConditions() { return true; }

        public abstract void Enter();
        public abstract void Exit();

        public abstract void Sleep();
        public abstract void Awake();

        protected virtual void UpdateAnimator() {}
        public abstract void OnUpdate();
        public abstract void OnLateUpdate();
        public abstract void OnFixedUpdate();

        public virtual void HandleAnimator() {}

        public virtual void OnAnimatorIK(int layerIndex) {}

        public void AddStateMode(BaseStateMode mode)
		{
            stateModes.Add (mode);
            mode.ModAdded();
		}

		/// <summary>
		/// Gets the state modes.
		/// </summary>
		/// <returns>The state modes.</returns>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public List<T> GetStateModes<T>() where T:BaseStateMode
		{
			List<T> modes = new List<T> ();

			foreach (BaseStateMode mode in stateModes) 
			{
				if (mode is T) 
				{
					modes.Add (mode as T);
				}
			}
			return modes;
		}

		/// <summary>
		/// Removes the state modes.
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public void RemoveStateModes<T>() where T:BaseStateMode
		{
			for (int i = 0; i < stateModes.Count; i++) 
			{
				if (stateModes [i] is T) 
				{
                    BaseStateMode mode = stateModes[i]; 
                    stateModes.RemoveAt (i);
                    mode.ModRemoved();
					i--;
				}
			}
		}
    }
}