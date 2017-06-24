using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.ChainProcessing;

public class AdditionChanLink : ChainLink
{
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
