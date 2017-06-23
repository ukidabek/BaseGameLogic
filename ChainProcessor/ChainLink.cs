using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	[Serializable]
	public abstract class ChainLink
	{
		public abstract ChainLink [] Inputs { get;}
		public abstract ChainLink [] Outputs { get;}
	}
}