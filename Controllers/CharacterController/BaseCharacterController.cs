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
    public abstract class BaseCharacterController : MonoBehaviour
    {
        public virtual bool IsPlayer { get { return false; } }

        protected virtual void Start() {}
                                                
		protected virtual void OnDestroy () {}
    }
}