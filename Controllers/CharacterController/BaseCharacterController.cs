using UnityEngine;
using UnityEngine.AI;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.States;

namespace BaseGameLogic.Character
{
    /// <summary>
    /// Base character controller.
    /// </summary>
    public abstract class BaseCharacterController : BaseStateObject
    {
		public virtual bool IsPlayer { get { return false; } }

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
        
		/// <summary>
        /// Character settings.
        /// </summary>
		[Header("Character settings & management.")]
		[SerializeField]
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
			
        protected virtual void Register()
        {
            if (CharacterRegisterInstance != null)
            {
                CharacterRegisterInstance.RegisterCharacter(this);
            }
            else
            {
                // exception
            }
        }


        protected override void Start()
        {
            base.Start();
            Register();
        }
                                                
		protected override void OnDestroy ()
    	{
			if (CharacterRegisterInstance != null) 
			{
				CharacterRegisterInstance.UnregisterCharacter (this);
			}
			else
			{
				// exception
			}
				
    		base.OnDestroy ();
    	}

		protected virtual CharacterStatus CreateCharacterStatus()
		{
			return new CharacterStatus ();
		}
    }
}