using UnityEngine;

using System;
using System.Collections;

public class NoAnimationHandlingDataCointainerException : Exception 
{
    public NoAnimationHandlingDataCointainerException(string cointainerName)
        : base(string.Format("Animation AnimationHandlingData does not cointains AnimationHandlingDataCointainer named: {0}. " +
            "Please defina such AnimationHandlingDataCointainer", cointainerName))
    {
    }
}
