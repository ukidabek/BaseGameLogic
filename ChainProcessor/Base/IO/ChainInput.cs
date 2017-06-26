using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class ChainInput : ChainLink 
	{
		private const string Link_Name = "ChainInput";
		public override string Name { get { return Link_Name; } }

		public ChainData InData = null;

		public override ChainLink[] Inputs  { get { return null;} }

		private ChainLink _output = null; 
		public override ChainLink[] Outputs 
		{
			get { return new ChainLink[]{_output}; }
		}

		public ChainInput (Vector2 position) : base (position) {}

		public override void Prosess ()
		{
			_output.Prosess ();
		}
	}
}