﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class ChainOutput : IOChainLink 
	{
		private const string Link_Name = "ChainOutput";
		public override string Name { get { return Link_Name; } }

		protected override int InputsCount { get { return 1; } }

		public ChainOutput (Vector2 position) : base (position) {}

		public override void Prosess ()
		{
			OutData = Inputs[0].OutData;
		}
	}
}