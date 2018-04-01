using UnityEngine;

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

        [SerializeField, Tooltip("List of state creators that this object can enter.")]
        protected List<BaseState> stateList = new List<BaseState>();

        [SerializeField, Tooltip("List of mode creators that can be apply to object states.")]
        protected List<BaseStateModeCreator> stateModesCreators = new List<BaseStateModeCreator>();

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
                if (statesStack.Count == 0) return null;

                return statesStack.Peek(); 
            }
        }

        #endregion

        #region Managers references

        protected bool GameManagerExist {get; private set; }
        protected BaseGameManager GameManagerInstance 
		{
			get { return BaseGameManager.Instance; }
		}

        /// <summary>
        /// Enable or disable execution of StateObject updates methods.
        /// </summary>
		protected bool IsGamePaused
		{
			get { return GameManagerExist && GameManagerInstance.GameStatus == GameStatusEnum.Pause; }
		}

        #endregion

        /// <summary>
        /// Enters the default state of the object.
        /// </summary>
        protected virtual void EnterDefaultState()
        {
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

        protected bool ExecuteModesFunction(StateModeUpdate update)
        {
            return true;
            bool additive = true;
            if (CurrentState == null)
                return additive;
            
			StateModeBlending blending = StateModeBlending.Additive;
			List<BaseStateMode> stateModes = CurrentState.StateModes;

			for (int i = 0; i < stateModes.Count; i++) 
			{
				BaseStateMode mode = stateModes [i];
				switch (update) 
				{
				case StateModeUpdate.Normal:
					blending = mode.Update ();
					break;
				case StateModeUpdate.Fixed:
					blending = mode.FixedUpdate ();
					break;
				case StateModeUpdate.Late:
					blending = mode.LateUpdate ();
					break;
				}

				if (blending == StateModeBlending.Override)
					additive = false;
			}

			return additive;
		}

        #region MonoBehaviour methods

        protected virtual void OnDestroy()
        {
            if(GameManagerExist)
                GameManagerInstance.ObjectInitializationCallBack.RemoveListener(InitializeObject);
        }

        protected virtual void Update ()
        {
			if (IsGamePaused)
				return;
			
			bool additive = ExecuteModesFunction (StateModeUpdate.Normal); 
			if(CurrentState != null && additive)
                CurrentState.OnUpdate();
        }

        protected virtual void LateUpdate()
        {
			if (IsGamePaused) return;
			
			bool additive = ExecuteModesFunction (StateModeUpdate.Late); 
			if(CurrentState != null && additive)
                CurrentState.OnLateUpdate();
        }

        protected virtual void FixedUpdate()
        {
			if (IsGamePaused) return;
			
			bool additive = ExecuteModesFunction (StateModeUpdate.Fixed); 
			if(CurrentState != null && additive)
                CurrentState.OnFixedUpdate();
        }

        public virtual void OnAnimatorIK(int layerIndex)
        {
            if (IsGamePaused) return;

            if (CurrentState != null)
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
                // else
                // {
                //     if(creator.ProductName.Equals(stateName))
                //     {
                //         return creator as T;
                //     }
                // }
            }

            #if UNITY_EDITOR
            if(state == null)
                Debug.LogErrorFormat("No creator for {0} in {1}", stateName, this.gameObject.name); 
            #endif

            return state as T;
        }

        /// <summary>
        /// Return a StateModeCreator added to BaseStateObject by it's name.
        /// </summary>
        /// <param name="stateModeName">Name of StateModeCreator </param>
        /// <returns></returns>
        // public BaseStateModeCreator GetBaseStateModeCreator(string stateModeName)
        // {
        //     foreach (BaseStateModeCreator creator in stateModesCreators)
        //     {
        //         if (creator != null && creator.ProductName.Equals(stateModeName))
        //             return creator;
        //     }

        //     #if UNITY_EDITOR
        //     Debug.LogErrorFormat("No creator for {0} in {1}", stateModeName, this.gameObject.name); 
        //     #endif

        //     return null;
        // }

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
                if (statesStack.Count > 0)
                {
                    CurrentState.OnSleep();
                }

                newState.ControlledObject = this;

                statesStack.Push(newState);
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