using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.ChainProcessing;

public class AdditionChanLink : ChainLink
{
	private const string Link_Name = "Addition";
	public override string Name { get { return Link_Name; } }
	public override Vector2 Size { get { return new Vector2 (100f, 100f); } }
	protected override int InputsCount { get { return 2;} }

	public AdditionChanLink (Vector2 position) : base (position) {}

	public override void Prosess ()
	{
		ChainData a = Inputs [0].OutData;
		ChainData b = Inputs [1].OutData;

		if (a != null && b != null) 
		{
			OutData = a + b;

			for (int i = 0; i < _outputs.Count; i++) 
			{
				_outputs [i].Prosess();
			}
		}
	}
}
