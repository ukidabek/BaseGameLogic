using UnityEngine;

using System;
using System.Collections;

namespace BaseGameLogic
{
    /// <summary>
    /// Animation events broadcaster.
    /// This object receives animation events and provides action to witch other ocject can sign up. 
    /// </summary>
    public abstract class AnimationEventsBroadcaster : MonoBehaviour 
    {
        #region Animation callbacks actions

        public Action animationStartCallbackAction;
        public Action onAnimationCallbackAction;
        public Action<string> onAnimationCallbackWithIDAction;
        public Action animationEndCallbackAction;

        #endregion

        #region Animation callbacks

        public void AnimationStartCallback()
        {
            if(animationStartCallbackAction != null)
                animationStartCallbackAction();
        }

        public void OnAnimationCallback()
        {
            if (onAnimationCallbackAction != null)
                onAnimationCallbackAction();
        }

        public void OnAnimationCallbackWithID(string eventID)
        {
            if (onAnimationCallbackWithIDAction != null)
                onAnimationCallbackWithIDAction(eventID);
        }

        public void AnimationEndCallback()
        {
            if(animationEndCallbackAction != null)
                animationEndCallbackAction();
        }

        #endregion
    }
}