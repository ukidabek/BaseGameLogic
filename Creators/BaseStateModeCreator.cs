using UnityEngine;
using System.Collections;

namespace BaseGameLogic.States
{
    /// <summary>
    /// Base state mode creator.
    /// Each state mode creator should inherit from this class.
    /// </summary>
    public abstract class BaseStateModeCreator : BaseCreator 
    {
        public abstract BaseStateMode CreateProduct(BaseState hostSate);
    }
}
