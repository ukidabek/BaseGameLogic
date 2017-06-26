using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public abstract class BaseChainLinkFactory : ScriptableObject 
	{
		public abstract string [] ChainLinkTypes { get; }

		public abstract ChainLink FabricateChainLink(Order order);
	}
}