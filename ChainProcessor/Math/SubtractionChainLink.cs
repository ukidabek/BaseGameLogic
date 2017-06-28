using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.ChainProcessing;

public class SubtractionChainLink : ChainLink 
{
	private const string Link_Name = "Subtraction";
	public override string Name { get { return Link_Name; } }

	protected override int InputsCount { get { return 2;} }

	public SubtractionChainLink (Vector2 position) : base (position) 
	{
		Vector2 size = new Vector2 (100, 100);
		_linkRect.size = size;
	}

	public override void Prosess ()
	{
		//		if (Inputs[0] != null || Inputs[1]!= null) 
		//		{
		//			if(_inputA != null && _inputB != null &&
		//				_inputA.OutData != null && _inputB.OutData != null)
		//			{
		//			}
		//		}
		ChainData a = Inputs [0].OutData;
		ChainData b = Inputs [1].OutData;

		if (a != null && b != null) 
		{
			OutData = a - b;

			for (int i = 0; i < _outputs.Count; i++) 
			{
				_outputs [i].Prosess();
			}
		}
	}
}
