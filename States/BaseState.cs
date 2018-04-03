using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Inputs;
using BaseGameLogic.Audio;
using System.Reflection;
using System;
using BaseGameLogic.LogicModule;

namespace BaseGameLogic.States
{
    /// <summary>
    /// Base state.
    /// </summary>
    public abstract class BaseState : MonoBehaviour
    {
		protected List<BaseStateMode> stateModes = new List<BaseStateMode> ();
		public List<BaseStateMode> StateModes
        {
    		get { return this.stateModes; }
    	}

        private BaseStateHandler controlledObject = null;
		public BaseStateHandler ControlledObject
        {
    		get { return this.controlledObject; }
			set { controlledObject = value; }
    	}

		private FieldInfo[] requiredFieldList = null;
		public Transform rootParent { get; private set; }

		protected virtual void Awake() 
		{
			rootParent = this.transform.GetRootTransform();
			requiredFieldList = GetAllRequiredFields();
		}

        public FieldInfo[] GetAllRequiredFields()
        {
			return AssemblyExtension.GetAllFieldsWithAttribute<RequiredReferenceAttribute>(this.GetType());
        }

		/// <summary>
		/// Get all references to fields marked with RequiredReference attribute 
		/// </summary>
		/// <param name="parent"></param>
		public void GetAllRequiredReferences(GameObject parent = null, bool overrideReference = false)
		{
			parent = parent == null ? this.transform.GetRootTransform().gameObject : parent;
			requiredFieldList = requiredFieldList == null ? GetAllRequiredFields() : requiredFieldList;
			BaseLogicModulesHandler handler = parent.GetComponentDeep<BaseLogicModulesHandler>();
			
			foreach (var item in requiredFieldList)
			{
				if(item.GetValue(this) == null || overrideReference)
				{
					Component component = handler.GetModule(item.FieldType);
					item.SetValue(this, component);
				}
			}
		}

        public virtual bool EnterConditions() { return true; }
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnSleep();
        public abstract void OnAwake();
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