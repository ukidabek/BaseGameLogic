using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class ChainOutput : IOChainLink 
	{
		private const string Link_Name = "ChainOutput";
		public override string Name { get { return Link_Name; } }

		private ChainLink _input = null; 
		public override ChainLink[] Inputs { get { return new ChainLink[]{_input}; }}

		public ChainOutput (Vector2 position) : base (position) {}

		public override void Prosess ()
		{
			OutData = _input.OutData;
		}
	}
}