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

		protected virtual ChainLink GetChainInput(Order order)
		{
			return order.LinkContainerObject.AddComponent<ChainInput>(); 
		}

		protected virtual ChainLink GetChainOutput(Order order)
		{
			return order.LinkContainerObject.AddComponent<ChainOutput>(); 
		}

		public virtual ChainLink FabricateChainLink(Order order)
		{
			ChainLink link = null;
			switch (order.Type) 
			{
			case INPUT:
				link = GetChainInput (order);
				break;

			case OUTPUT:
				link = GetChainOutput (order);
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