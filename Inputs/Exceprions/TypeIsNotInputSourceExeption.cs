using UnityEngine;

using System;

namespace BaseGameLogic.Inputs
{
    public class TypeIsNotInputSourceException : Exception
    {
        public TypeIsNotInputSourceException() : base("Provided type is not subclass of BaseInputSource.") {}
    }
}