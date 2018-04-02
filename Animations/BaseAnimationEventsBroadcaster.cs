using UnityEngine;
using UnityEngine.Events;


using System;
using System.Collections;

namespace BaseGameLogic.Animation
{
    using Object = UnityEngine.Object;
    
    /// <summary>
    /// Animation events broadcaster.
    /// This object receives animation events and provides action to witch other object can sign up. 
    /// </summary>
    public abstract class BaseAnimationEventsBroadcaster : MonoBehaviour 
    {
        #region Animation callbacks actions

        public UnityEvent OnAnimationStart = new UnityEvent();
        public UnityEvent OnAnimationPlay = new UnityEvent();
        public StringUnityEvent OnAnimationPlayWithString = new StringUnityEvent();
        public IntUnityEvent OnAnimationPlayWithInt = new IntUnityEvent();
        public FloatUnityEvent OnAnimationPlayWithFloat = new FloatUnityEvent();
        public ObjectUnityEvent OnAnimationPlayWithObject = new ObjectUnityEvent();
        public UnityEvent OnAnimationEnd = new UnityEvent();


        #endregion

        #region Animation callbacks

        public void AnimationStartCallback()
        {
            OnAnimationStart.Invoke();
        }

        public void OnAnimationCallback()
        {
            OnAnimationPlay.Invoke();
        }

        public void OnAnimationCallbackString(string data)
        {
            OnAnimationPlayWithString.Invoke(data);
        }

        public void OnAnimationCallbackInt(int data)
        {
            OnAnimationPlayWithInt.Invoke(data);
        }

        public void OnAnimationCallbackFloat(float data)
        {
            OnAnimationPlayWithFloat.Invoke(data);
        }

        public void OnAnimationCallbackObject(Object data)
        {
            OnAnimationPlayWithObject.Invoke(data);
        }

        public void AnimationEndCallback()
        {
            OnAnimationEnd.Invoke();
        }

        #endregion
    }
}