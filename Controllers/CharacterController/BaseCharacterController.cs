﻿using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Inputs;

namespace BaseGameLogic
{
    /// <summary>
    /// Base character controller.
    /// </summary>
//    [RequireComponent(typeof(InputCollector))]
    public class BaseCharacterController : BaseStateObject
    {
		#region Managers references

		protected CharacterRegister CharacterRegisterInstance
		{
			get 
			{ 
				if(GameManagerInstance != null)
					return GameManagerInstance.CharacterRegisterInstance;

				return null;
			}
		}

		#endregion

		#region Components references

		protected CharacterController characterController = null;
		public CharacterController CharacterController 
		{
			get { return this.characterController; } 
		}

		protected NavMeshAgent characterNavMeshAgent = null;
		public NavMeshAgent CharacterNavMeshAgent 
		{
			get { return this.characterNavMeshAgent; } 
		}

		#endregion

		#region Runtime variables

		[SerializeField]
		protected GameObject _meshReference;
		public GameObject MeshReference {
			get { return _meshReference; }
		}

		[SerializeField]
		protected float characterXRotationAngle = 0f; 
		public float CharacterXRotationAngle {
    		get { return this.characterXRotationAngle; }
    		set { characterXRotationAngle = value; }
    	}

		[SerializeField]
		protected float characterYRotationAngle = 0f;
		public float CharacterYRotationAngle {
    		get { return this.characterYRotationAngle; }
    		set { characterYRotationAngle = value; }
    	}

		#endregion
        
		/// <summary>
        /// Character settings.
        /// </summary>
		[Header("Character settings & managment.")]
		[SerializeField, Tooltip("Slot for CharacterSettings instance.")]
        protected CharacterSettings settings = null;
        public CharacterSettings Settings 
        {
            get { return this.settings; }
            set { settings = value; }
        }

		private CharacterStatus characterStatus = null;
		public CharacterStatus CharacterStatus 
		{
    		get 
			{
				if (characterStatus == null) 
				{
					characterStatus = CreateCharacterStatus ();
				}

				return this.characterStatus; 
			}
    	}
			
        protected override void Awake ()
        {
            base.Awake ();

            characterController = this.gameObject.GetComponent<CharacterController>();
            #if UNITY_EDITOR
            MissingWarning(characterController, gameObject.name);
            #endif

            characterNavMeshAgent = this.GetComponent<NavMeshAgent>();
            #if UNITY_EDITOR
            MissingWarning(characterNavMeshAgent, gameObject.name);
            #endif
        }
            
        protected override void Start()
        {
            base.Start();

			if (CharacterRegisterInstance != null) 
			{
				CharacterRegisterInstance.RegisterCharacter (this);
			}
			else
			{
				// exeption
			}
		}
                                                
        protected virtual void OnAnimatorIK(int layerIndex)
        {
        }

		protected override void OnDestroy ()
    	{
			if (CharacterRegisterInstance != null) 
			{
				CharacterRegisterInstance.UnregisterCharacter (this);
			}
			else
			{
				// exeption
			}
				
    		base.OnDestroy ();
    	}

		protected virtual CharacterStatus CreateCharacterStatus()
		{
			return new CharacterStatus ();
		}
    }
}