using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.States
{
    public abstract class BaseStateTransitionCondition : MonoBehaviour
    {
        public abstract bool Validate();
    }
}