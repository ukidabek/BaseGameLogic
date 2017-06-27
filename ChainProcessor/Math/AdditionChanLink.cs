using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.ChainProcessing;

public class AdditionChanLink : ChainLink
{
	private const string Link_Name = "Addition";
	public override string Name { get { return Link_Name; } }

	[SerializeField]
	private ChainLink _inputA = null;
	[SerializeField]
	private ChainLink _inputB = null;
	public override ChainLink[] Inputs 
	{
		get { return new ChainLink [] { _inputA, _inputB }; }
	}

	public AdditionChanLink (Vector2 position) : base (position) 
	{
		Vector2 size = new Vector2 (100, 100);
		_linkRect.size = size;
	}

	public override void Prosess ()
	{
		if (_inputA != null || _inputB != null) 
		{
			if(_inputA != null && _inputB != null &&
				_inputA.OutData != null && _inputB.OutData != null)
			{
			}
		}
	}
}
