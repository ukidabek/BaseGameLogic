using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.ChainProcessing;

public class AdditionChanLink : ChainLink
{
	private const string Link_Name = "Addition";
	public override string Name { get { return Link_Name; } }

	private ChainLink inputA = null;
	private ChainLink inputB = null;

	public override ChainLink[] Inputs 
	{
		get { return new ChainLink [] { inputA, inputB }; }
	}

	public override ChainLink[] Outputs 
	{
		get { throw new System.NotImplementedException (); }
	}

	public AdditionChanLink (Vector2 position) : base (position) 
	{
		Vector2 size = new Vector2 (100, 100);
		_linkRect.size = size;
	}

	public override void Prosess ()
	{
		if (inputA != null || inputB != null) 
		{
			if(inputA != null && inputB != null &&
				inputA.OutData != null && inputB.OutData != null)
			{
			}
		}
	}
}
