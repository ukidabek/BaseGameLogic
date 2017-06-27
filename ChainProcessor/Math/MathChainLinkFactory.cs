using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.ChainProcessing;

[CreateAssetMenu(
	fileName = "MathChainLinkFactory", 
	menuName = "BaseGameLogic/ChainLinkFactories/Math")]
public class MathChainLinkFactory : BaseChainLinkFactory
{
	private const string ADDITION = "Addition";
	private const string SUBTRACTION = "Subtraction";
	private const string MULTIPLICATION = "Multiplication";
	private const string DIVISION = "Division";

	private string [] _chainLinkTypes = new string[] { 
		INPUT,
		OUTPUT,
		ADDITION, 
		SUBTRACTION, 
		MULTIPLICATION, 
		DIVISION 
	};

	public override string[] ChainLinkTypes 
	{
		get { return this._chainLinkTypes; }
	}

	public override ChainLink FabricateChainLink (Order order)
	{
		ChainLink link = base.FabricateChainLink(order);
		if (link != null)
			return link;
		
		switch (order.Type) 
		{
		case ADDITION:
			link = order.LinkContainerObject.AddComponent<AdditionChanLink>(); 
			break;
		case SUBTRACTION:
			break;
		case MULTIPLICATION:
			break;
		case DIVISION:
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
