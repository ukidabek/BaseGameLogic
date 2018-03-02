using UnityEngine;
using System.Collections;

using BaseGameLogic.Audio;

namespace BaseGameLogic.States
{
    /// <summary>
    /// Base state creator.
    /// Each state creator should inherit from this class.
    /// </summary>
    public abstract class BaseStateCreator : BaseCreator 
    { 
        public abstract BaseState CreateProduct(BaseStateObject controlledObject); 
    }
}
