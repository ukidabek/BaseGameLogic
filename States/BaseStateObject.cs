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
    public abstract class BaseStateObject : MonoBehaviour
    {
        #if UNITY_EDITOR

        [Header("Debug display & options.")]
        [SerializeField, Tooltip("Visualizes the contents of the stack.")]
        protected List<string> currentStateTypes = new List<string>();

        #endif

        #region States management variables

		[Header("States management.")]
        [SerializeField, Tooltip("Default state creator - object that create states.")]
        protected BaseStateCreator defaultStateCreator = null;

        [SerializeField, Tooltip("List of state creators that this object can enter.")]
        protected List<BaseStateCreator> stateCreators = new List<BaseStateCreator>();

        [SerializeField, Tooltip("List of mode creators that can be apply to object states.")]
        protected List<BaseStateModeCreator> stateModesCreators = new List<BaseStateModeCreator>();

        [SerializeField]
        private BaseLogicModulesHandler _logicModulesHandler = null;
        public BaseLogicModulesHandler LogicModulesHandler { get { return _logicModulesHandler; } }

        /// <summary>
        /// Stack of states.
        /// </summary>
        protected Stack<BaseState> states = new Stack<BaseState>();

        /// <summary>
        /// Reference to current state of the object.
        /// </summary>
        public BaseState CurrentState
        {
            get 
            {
                if (states.Count == 0) return null;

                return states.Peek(); 
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
            if (defaultStateCreator == null)
                return;

            BaseState defaultState = null;

            if (defaultStateCreator != null)
            {
                defaultState = defaultStateCreator.CreateProduct(this);
                
                this.EnterState(defaultState);
            }
        }

        protected virtual void Awake() { }

        protected virtual void Start () 
        {
            GameManagerExist = GameManagerInstance != null;
            if(GameManagerExist)
            {
			    GameManagerInstance.ObjectInitializationCallBack -= InitializeObject;
			    GameManagerInstance.ObjectInitializationCallBack += InitializeObject;
            }
        }

		/// <summary>
		/// This method is called by GameManager in first update of this object.
		/// </summary>
		protected virtual void InitializeObject()
        {
            EnterDefaultState();
        }

        protected bool ExecuteModesFunction(StateModeUpdate update)
        {
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
            {
                GameManagerInstance.ObjectInitializationCallBack -= InitializeObject;
            }
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

        public virtual void OnCollisionEnter(Collision collision) {}

        public virtual void OnCollisionStay(Collision collision) {}

        public virtual void OnCollisionExit(Collision collision) {}

        public virtual void OnTriggerEnter(Collider collision) {}

        public virtual void OnTriggerStay(Collider collision) {}

        public virtual void OnTriggerExit(Collider collision) {}

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
        public T GetStateCreator<T>(string stateName = "") where T:BaseStateCreator
        {
            foreach (BaseStateCreator creator in stateCreators)
            {
                if (creator is T && string.IsNullOrEmpty(stateName))
                {
                    return creator as T;
                }
                else
                {
                    if(creator.ProductName.Equals(stateName))
                    {
                        return creator as T;
                    }
                }
            }

            #if UNITY_EDITOR
            Debug.LogErrorFormat("No creator for {0} in {1}", stateName, this.gameObject.name); 
            #endif

            return null;
        }

        /// <summary>
        /// Return a StateModeCreator added to BaseStateObject by it's name.
        /// </summary>
        /// <param name="stateModeName">Name of StateModeCreator </param>
        /// <returns></returns>
        public BaseStateModeCreator GetBaseStateModeCreator(string stateModeName)
        {
            foreach (BaseStateModeCreator creator in stateModesCreators)
            {
                if (creator != null && creator.ProductName.Equals(stateModeName))
                    return creator;
            }

            #if UNITY_EDITOR
            Debug.LogErrorFormat("No creator for {0} in {1}", stateModeName, this.gameObject.name); 
            #endif

            return null;
        }

        /// <summary>
        /// Enter a new state. 
        /// </summary>
        /// <param name="newState"> New state instance.</param>
        public void EnterState(BaseState newState)
        {
            if (newState.EnterConditions())
            {
                if (states.Count > 0)
                {
                    CurrentState.Sleep();
                }

                states.Push(newState);
                CurrentState.Enter();

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
        /// Exits current state. 
        /// </summary>
        public void ExitState()
        {
            states.Pop().Exit();
            CurrentState.Awake();

            #if UNITY_EDITOR
            currentStateTypes.RemoveAt(0);
            #endif    
        }

        #if UNITY_EDITOR
        public void GetLogicModuleHandler()
        {
            _logicModulesHandler = gameObject.GetComponent<BaseLogicModulesHandler>();
        }

        public void AddLogicModuleHandler()
        {
            Type[] types = AssemblyExtension.GetDerivedTypes<BaseLogicModulesHandler>();
            if(types == null && types.Length > 0)
                _logicModulesHandler = gameObject.AddComponent(types[0]) as BaseLogicModulesHandler;
            else
                Debug.LogError("There is no class that extend BaseLogicModulesHandler.");
        }

        public void RemoveLogicModuleHandler()
        {
            DestroyImmediate(_logicModulesHandler);
        }

        #endif
    }
}