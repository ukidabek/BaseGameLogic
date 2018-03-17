using UnityEngine;

using System;
using System.Collections;

namespace BaseGameLogic.Inputs
{
    public class NoPhysicalInputException : Exception 
    {
		public NoPhysicalInputException(string PhysicalInputName, Type type)
            : base(string.Format("InputSource does not contains PhysicalInput named: {0} type of {1}. " +
			"Please defina such PhysicalInput.", PhysicalInputName, type.ToString())) {}
    }
}