using UnityEngine;

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Inputs;
using BaseGameLogic.Audio;

using BaseGameLogic.LogicModule;

namespace BaseGameLogic.States
{
    /// <summary>
    /// Base state.
    /// </summary>
    public abstract class BaseState : MonoBehaviour
    {
        private BaseStateHandler controlledObject = null;
		public BaseStateHandler ControlledObject
        {
    		get { return this.controlledObject; }
			set { controlledObject = value; }
    	}

		private FieldInfo[] requiredFieldList = null;

		public Transform RootParent { get; private set; }

        [SerializeField] private List<StateTransition> _stateTransition = new List<StateTransition>();
        public List<StateTransition> Transitions { get { return _stateTransition; } }

        [SerializeField] private List<BaseStateTransitionCondition> _exitStateConditons = new List<BaseStateTransitionCondition>();
        public List<BaseStateTransitionCondition> ExitStateConditons { get { return _exitStateConditons; } }


        protected virtual void Awake() 
		{
			RootParent = this.transform.GetRootTransform();
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
			BaseLogicModulesHandler handler = parent.GetComponentDeep<BaseLogicModulesHandler>();

            GetAllRequiredReferences(handler, overrideReference);
        }

        public void GetAllRequiredReferences(BaseLogicModulesHandler handler, bool overrideReference = false)
        {
			requiredFieldList = requiredFieldList == null ? GetAllRequiredFields() : requiredFieldList;

            foreach (FieldInfo field in requiredFieldList)
            {
                if (overrideReference || field.GetValue(this) == null)
                {
                    field.SetValue(this, handler.GetModule(field.FieldType));
                }
            }
        }

        public virtual bool EnterConditions() { return true; }
        public abstract void OnEnter();
        public abstract void OnExit();
        public abstract void OnSleep();
        public abstract void OnAwake();
        public virtual void UpdateAnimator() {}
        public abstract void OnUpdate();
        public abstract void OnLateUpdate();
        public abstract void OnFixedUpdate();
        public virtual void HandleAnimator() {}
        public virtual void OnAnimatorIK(int layerIndex) {}
    }
}