﻿using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;
using BaseGameLogic.Inputs;
using BaseGameLogic.Audio;
using BaseGameLogic.LogicModule;

namespace BaseGameLogic.States
{
    /// <summary>
    /// Base game object.
    /// </summary>
    public abstract class BaseStateHandler : MonoBehaviour
    {
        #if UNITY_EDITOR

        [Header("Debug display & options.")]
        [SerializeField, Tooltip("Visualizes the contents of the stack.")]
        protected List<string> currentStateTypes = new List<string>();

        #endif

        #region States management variables

		[Header("States management.")]
        [SerializeField]
        protected bool enterDefaultStateOnAwake = false;
        [SerializeField, Tooltip("Default state creator - object that create states.")]
        protected BaseState defaultState = null;

        [SerializeField] protected BaseStateGraph _graph = null;

        [SerializeField, Tooltip("List of state creators that this object can enter.")]
        protected List<BaseState> stateList = new List<BaseState>();

        protected BaseState _currentState = null;
        /// <summary>
        /// Stack of states.
        /// </summary>
        protected Stack<BaseState> statesStack = new Stack<BaseState>();

        /// <summary>
        /// Reference to current state of the object.
        /// </summary>
        public BaseState CurrentState
        {
            get 
            {
                switch (_graph.Type)
                {
                    case GraphType.Stack:
                        if (statesStack.Count == 0) return null;
                        return statesStack.Peek();

                    case GraphType.Free:
                        return _currentState;
                }

                return null;
            }
        }

        #endregion

        #region Managers references

        protected bool GameManagerExist { get; private set; }
        protected BaseGameManager GameManagerInstance { get { return BaseGameManager.Instance; } }

        /// <summary>
        /// Enable or disable execution of StateObject updates methods.
        /// </summary>
		protected bool IsGamePaused { get { return GameManagerExist && GameManagerInstance.GameStatus == GameStatusEnum.Pause; } }

        #endregion

        /// <summary>
        /// Enters the default state of the object.
        /// </summary>
        protected virtual void EnterDefaultState()
        {
            if(_graph != null)
            {
                this.EnterState(_graph.RootState);
                return;
            }

            if (defaultState == null) 
            {
                Debug.LogWarning("There is no default state set.");
                return;
            }

            this.EnterState(defaultState);
        }

        protected virtual void Awake() 
        { 
            if(enterDefaultStateOnAwake)
                EnterDefaultState();
        }

        protected virtual void Start () 
        {
            GameManagerExist = GameManagerInstance != null;
            if(GameManagerExist)
			    GameManagerInstance.ObjectInitializationCallBack.AddListener(InitializeObject);
        }

		/// <summary>
		/// This method is called by GameManager in first update of this object.
		/// </summary>
		protected virtual void InitializeObject(BaseGameManager gameManager)
        {
            if(!enterDefaultStateOnAwake)
                EnterDefaultState();
        }

        #region MonoBehaviour methods

        protected virtual void OnDestroy()
        {
            if(GameManagerExist)
                GameManagerInstance.ObjectInitializationCallBack.RemoveListener(InitializeObject);
        }

        protected virtual void Update ()
        {
			if (IsGamePaused && CurrentState != null)
				return;

            if(_graph != null)
                _graph.HandleTransitions(this);

			CurrentState.OnUpdate();
        }

        protected virtual void LateUpdate()
        {
			if (IsGamePaused && CurrentState != null) 
                return;
			
			CurrentState.OnLateUpdate();
        }

        protected virtual void FixedUpdate()
        {
			if (IsGamePaused && CurrentState != null) 
                return;
			
			CurrentState.OnFixedUpdate();
        }

        public virtual void OnAnimatorIK(int layerIndex)
        {
            if (IsGamePaused && CurrentState != null) 
                return;

            CurrentState.OnAnimatorIK(layerIndex);
        }

        #endregion

        /// <summary>
        /// Return a StateCreator added to BaseStateObject by it's name.
        /// </summary>
        /// <param name="stateName">Name of StateCreator.</param>
        /// <returns></returns>
        public T GetState<T>(string stateName = "") where T:BaseState
        {
            BaseState state = null;
            foreach (BaseState creator in stateList)
            {
                if (creator is T && string.IsNullOrEmpty(stateName))
                {
                    state = creator;
                }
            }

            #if UNITY_EDITOR
            if(state == null)
                Debug.LogErrorFormat("No creator for {0} in {1}", stateName, this.gameObject.name); 
            #endif

            return state as T;
        }

        /// <summary>
        /// Enter a new state. 
        /// </summary>
        /// <param name="newState"> New state instance.</param>
        public void EnterState(BaseState newState)
        {
            if(newState == null)
                return;

            if (newState.EnterConditions())
            {
                if(_graph.Type == GraphType.Stack)
                {
                    if (statesStack.Count > 0)
                        CurrentState.OnSleep();
                    
                    statesStack.Push(newState);
                }
                else
                {
                    _currentState = newState;
                }

                newState.ControlledObject = this;
                newState.GetAllRequiredReferences(this.gameObject, true);
                CurrentState.OnEnter();

                #if UNITY_EDITOR
				currentStateTypes.Insert(0, newState.GetType().Name);
                #endif    
            }
            else
            {
                string typeName = newState.GetType().Name;
                Debug.LogErrorFormat("Conditions to enter the state type of {0} were not met.", typeName);
            }
        }

        /// <summary>
        /// Enter to a net state using State creator of type T
        /// </summary>
        public void EnterState<T>() where T:BaseState
        {
            BaseState state = GetState<T>();
            if(state != null)
                EnterState(state);
        }

        /// <summary>
        /// Exits current state. 
        /// </summary>
        public void ExitState()
        {
            if (_graph.Type == GraphType.Free)
                return;
            BaseState oldState = statesStack.Pop();
            oldState.ControlledObject = null;
            oldState.OnExit();
            CurrentState.OnAwake();

            #if UNITY_EDITOR
            currentStateTypes.RemoveAt(0);
            #endif    
        }
    }
}