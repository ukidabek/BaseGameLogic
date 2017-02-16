using UnityEngine;

using System;
using System.Collections;

namespace BaseGameLogic
{
    [Serializable]
    public class AnimationHandlingDataContainer
    {
        [SerializeField]
        public string cointainerName = string.Empty;

        [SerializeField]
        public string parameterName = string.Empty;

        [SerializeField]
        public int parameterNameHashID = 0;

        public void InitializeParameter()
        {
            parameterNameHashID = Animator.StringToHash(parameterName);
        }

        /// <summary>
        /// Sets the trigger.
        /// </summary>
        /// <param name="animator">Animator.</param>
        public void SetParameter(Animator animator)
        {
            animator.SetTrigger(parameterNameHashID);
        }

        /// <summary>
        /// Sets the bool.
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="value">If set to <c>true</c> value.</param>
        public void SetParameter(Animator animator, bool value)
        {
            animator.SetBool(parameterNameHashID, value);
        }

        /// <summary>
        /// Sets the float.
        /// </summary>
        /// <param name="animator">Animator.</param>
        /// <param name="value">Value.</param>
        public void SetParameter(Animator animator, float value)
        {
            animator.SetFloat(parameterNameHashID, value);
        }
    }
}
