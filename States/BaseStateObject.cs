using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Inputs;
using BaseGameLogic.Audio;
using BaseGameLogic.Events;

namespace BaseGameLogic
{
    /// <summary>
    /// Base game object.
    /// </summary>
    public class BaseStateObject : MonoBehaviour, IEventClient// BaseGameLogic.SaveLoadSystem.SaveStructure
    {   
        #if UNITY_EDITOR

		[Header("Debug display & options.")]
        [SerializeField, Tooltip("Visualizes the contents of the stack.")]
        protected List<string> currentStateTypes = new List<string>();

        [SerializeField, Tooltip("Show reference validation log? ")]
        protected bool showReferenceValidationLog = true;

        #endif

        #region States managment variables

		[Header("States managment.")]
        [SerializeField, Tooltip("Default state creator - object that create states.")]
        protected BaseStateCreator defaultStateCreator = null;

        [SerializeField, Tooltip("List of state creatorsof that this object can enter.")]
        protected List<BaseStateCreator> stateCreators = new List<BaseStateCreator>();

        [SerializeField, Tooltip("List of mode creators that can be applyed to object states.")]
        protected List<BaseStateModeCreator> stateModesCreators = new List<BaseStateModeCreator>();

        /// <summary>
        /// Stack of states.
        /// </summary>
        [SerializeField]
        protected Stack<BaseState> states = new Stack<BaseState>();

        public BaseState CurrentState
        {
            get 
            {
                if (states.Count == 0)
                    return null;

                return states.Peek(); 
            }
        }

        #endregion

		#region Managers references

		protected GameManager GameManagerInstance 
		{
			get { return GameManager.Instance; }
		}

        public EventManager EventManagerInstance
        {
            get
            {
                return GameManager.Instance.EventManagerInstance;
            }
        }

		protected virtual bool EventCanBeRegistred
		{
			get { return EventManagerInstance != null; }
		}

		protected bool IsGamePaused
		{
			get 
			{
				return GameManagerInstance != null && 
					GameManagerInstance.GameStatus == GameStatusEnum.Pause;
			}
		}

		#endregion

        #region Components references

        protected Rigidbody objectRigidbody = null;
        public Rigidbody ObjectRigidbody {
            get { return this.objectRigidbody; } 
        }

        /// <summary>
        /// The animation events broadcaster.
        /// </summary>
        protected AnimationEventsBroadcaster animationEventsBroadcaster = null;
        public AnimationEventsBroadcaster AnimationEventsBroadcaster
        {
            get { return this.animationEventsBroadcaster; }
        }
       
        /// <summary>
        /// The sound effect manager.
        /// </summary>
        protected SoundEffectManager soundEffectManager = null;
        public SoundEffectManager SoundEffectManager
        {
            get { return this.soundEffectManager; }
        }

        /// <summary>
        /// The object animator.
        /// </summary>
        protected Animator objectAnimator = null;
        public Animator ObjectAnimator {
            get { return this.objectAnimator; } 
        }
			
        /// <summary>
        /// The input collector.
        /// Contains data on inputs.
        /// </summary>
        public InputCollector InputCollector 
        {
            get 
			{ 
				if (GameManagerInstance == null) 
				{
					return null;
				}

				return this.GameManagerInstance.InputCollectorInstance; 
			} 
        }

		/// <summary>
		/// The animation handling data.
		/// Contains data enabling animator control.
		/// </summary>
		[Header("Object settings & managment.")]
		[SerializeField]
		protected AnimationHandlingData animationHandlingData = null;
		public AnimationHandlingData AnimationHandlingData {
			get { return this.animationHandlingData; }
		}

		#endregion

        #region Object Caches

        /// <summary>
        /// The input cache contains references to physical inputs for faster acces.
        /// </summary>
        private  BaseInputCache inputCache = null;
        public BaseInputCache InputCache
        {
            get 
            { 
                if (inputCache == null)
                {
                    inputCache = CreateInputCache();
                }
                return this.inputCache; 
            }
        }

        /// <summary>
        /// The animation handling cache contains references to animation data containers for faster acces.
        /// </summary>
        private BaseAnimationHandlingCache animationHandlingCache = null;
        public BaseAnimationHandlingCache AnimationHandlingCache
        {
            get
            {
                if (animationHandlingCache == null)
                {
                    animationHandlingCache = CreateAnimationHandlingCache();
                }

                return this.animationHandlingCache;
            }
        }

        #endregion
            
        /// <summary>
        /// Creates the input cache.
        /// </summary>
        /// <returns>The input cache.</returns>
        protected virtual BaseInputCache CreateInputCache()
        {
            return new BaseInputCache();
        }

        /// <summary>
        /// Creates the animation handling cache.
        /// </summary>
        /// <returns>The animation handling cache.</returns>
        protected virtual BaseAnimationHandlingCache CreateAnimationHandlingCache()
        {
            return new BaseAnimationHandlingCache();
        }

        #if UNITY_EDITOR            

        protected void MissingWarning<T>(T obj, string gameObjectName)
        {
            
            if (showReferenceValidationLog && obj == null)
            {
                Type type = typeof(T);
                Debug.LogWarningFormat("{0} is missing for {1}.", type.ToString(), this.gameObject.name);
            }
        }

        #endif

        protected virtual void Awake() 
        {
            #if UNITY_EDITOR            
			MissingWarning(InputCollector, gameObject.name);
            #endif

            this.objectAnimator = gameObject.GetComponent<Animator>();
            #if UNITY_EDITOR
            MissingWarning(objectAnimator, gameObject.name);
            #endif

            this.animationEventsBroadcaster = this.gameObject.GetComponent<AnimationEventsBroadcaster>();
            #if UNITY_EDITOR
            MissingWarning(animationEventsBroadcaster, gameObject.name);
            #endif

            this.soundEffectManager = this.gameObject.GetComponent<SoundEffectManager>();
            #if UNITY_EDITOR
            MissingWarning(soundEffectManager, gameObject.name);
            #endif

            this.objectRigidbody = this.gameObject.GetComponent<Rigidbody>();
            #if UNITY_EDITOR
            MissingWarning(objectRigidbody, gameObject.name);
            #endif
        }

		/// <summary>
		/// Enters the default state.
		/// </summary>
        protected virtual void EnterDefaultState()
        {
            if (defaultStateCreator == null)
                return;


            BaseState defaultState = null;

            if (defaultStateCreator != null)
            {
                defaultState = defaultStateCreator.CreateProduct(
                    this,
                    InputCache,
                    AnimationHandlingCache);
                
                this.EnterState(defaultState);
            }
        }

        protected virtual void Start () 
        {
            EventClientStart();
            EnterDefaultState();
        }

		protected bool ExecutModesFunction(StateModeUpdate update)
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

        protected virtual void Update ()
        {
			if (IsGamePaused)
				return;
			
			bool additive = ExecutModesFunction (StateModeUpdate.Normal); 
			if(CurrentState != null && additive)
                CurrentState.OnUpdate();
        }

        protected virtual void LateUpdate()
        {
			if (IsGamePaused)
				return;
			
			bool additive = ExecutModesFunction (StateModeUpdate.Late); 
			if(CurrentState != null && additive)
                CurrentState.OnLateUpdate();
        }

        protected virtual void FixedUpdate()
        {
			if (IsGamePaused)
				return;
			
			bool additive = ExecutModesFunction (StateModeUpdate.Fixed); 
			if(CurrentState != null && additive)
                CurrentState.OnFixedUpdate();
        }

        public BaseStateCreator GetStateCreator(string stateName)
        {
            foreach (BaseStateCreator creator in stateCreators)
            {
                if (creator != null &&
                    creator.ProductName.Equals(stateName))
                    return creator;
            }

            #if UNITY_EDITOR
            Debug.LogErrorFormat("No creator for {0} in {1}", stateName, this.gameObject.name); 
            #endif

            return null;
        }

        public BaseStateModeCreator GetBaseStateModeCreator(string stateModeName)
        {
            foreach (BaseStateModeCreator creator in stateModesCreators)
            {
                if (creator != null &&
                    creator.ProductName.Equals(stateModeName))
                    return creator;
            }

            #if UNITY_EDITOR
            Debug.LogErrorFormat("No creator for {0} in {1}", stateModeName, this.gameObject.name); 
            #endif

            return null;
        }


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
                currentStateTypes.Insert(0, newState.GetType().ToString());
                #endif    
            }
        }

        public void ExitState()
        {
            states.Pop().Exit();
            CurrentState.Awake();

            #if UNITY_EDITOR
            currentStateTypes.RemoveAt(0);
            #endif    
        }

		protected virtual void OnDestroy() 
		{
			EventClientOnDestroy ();
			string name = this.gameObject.name;
			Debug.LogFormat ("{0} was destroyed", name); 
		}

        public virtual void EventClientStart ()
		{
			RegisterAllEvents ();
		}

        public virtual void EventClientOnDestroy ()
		{
			UnregisterAllEvents ();
		}

        public void RegisterAllEvents()
        {
        }

        public void UnregisterAllEvents()
        {
        }

        public virtual void OnCollisionEnter(Collision collision)
        {
        }

		public virtual void OnCollisionStay(Collision collision)
		{
		}

		public virtual void OnCollisionExit(Collision collision) 
        {
        }

        public virtual void OnTriggerEnter(Collider collision)
        {
        }

		public virtual void OnTriggernStay(Collider collision)
		{
		}

        public virtual void OnTriggerExit(Collider collision) 
        {
        }

    }
}