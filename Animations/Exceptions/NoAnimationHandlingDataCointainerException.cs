
using System;

/// <summary>
/// No AnimationHandlingDataCointainer exception classs. 
/// Throw this exception if reference for your AnimationHandlingData is null.
/// </summary>
public class NoAnimationHandlingDataCointainerException : Exception 
{
    private const string Eeceprion_Information_Message = "Animation AnimationHandlingData does not cointains AnimationHandlingDataCointainer named: {0}. Please defina such AnimationHandlingDataCointainer";

    public NoAnimationHandlingDataCointainerException(string cointainerName)
        : base(string.Format(Eeceprion_Information_Message, cointainerName)) {}
}
