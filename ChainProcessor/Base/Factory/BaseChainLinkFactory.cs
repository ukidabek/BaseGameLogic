using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public abstract class BaseChainLinkFactory : ScriptableObject 
	{
		protected const string INPUT= "Input";
		protected const string OUTPUT = "Output";

		public abstract string[] ChainLinkTypes { get; }

		public virtual ChainLink FabricateChainLink(Order order)
		{
			ChainLink link = null;
			switch (order.Type) 
			{
			case INPUT:
				link = order.LinkContainerObject.AddComponent<ChainInput>(); 
				break;
			case OUTPUT:
				link = order.LinkContainerObject.AddComponent<ChainOutput>(); 
				break;
			}

			if (link != null) 
			{
				Rect newRect = link.LinkRect;
				newRect.position = order.Position;
				Vector2 size = new Vector2 (100, 100);
				newRect.size = size;
				link.LinkRect = newRect;
			}

			return link;
		}
	}
}