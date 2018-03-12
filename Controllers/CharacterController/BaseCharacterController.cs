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

		protected CharacterRegister CharacterRegisterInstance { get { return CharacterRegister.Instance; } }
			
        protected virtual void Register()
        {
            if (CharacterRegisterInstance != null)
            {
                CharacterRegisterInstance.RegisterCharacter(this);
            }
        }

		protected virtual void Unregister()
		{
            if (CharacterRegisterInstance != null)
            {
                CharacterRegisterInstance.UnregisterCharacter(this);
            }
        }


        protected override void Start()
        {
            base.Start();
            Register();
        }
                                                
		protected override void OnDestroy ()
    	{
            Unregister();
            base.OnDestroy ();
    	}
    }
}