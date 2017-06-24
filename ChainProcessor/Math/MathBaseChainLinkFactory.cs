using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.ChainProcessing;

public class MathBaseChainLinkFactory : BaseChainLinkFactory
{
	private const string ADDITION = "Addition";
	private const string SUBTRACTION = "Subtraction";
	private const string MULTIPLICATION = "Multiplication";
	private const string DIVISION = "Division";

	private string [] _chainLinkTypes = new string[] { 
		ADDITION, 
		SUBTRACTION, 
		MULTIPLICATION, 
		DIVISION 
	};

	public override string[] ChainLinkTypes 
	{
		get { return _chainLinkTypes;}
	}

	public override ChainLink FabricateChainLink (string type)
	{
		switch (type) 
		{
		case ADDITION:
			return null;
		case SUBTRACTION:
			return null;
		case MULTIPLICATION:
			return null;
		case DIVISION:
			return null;
		}

		return null;
	}
}
