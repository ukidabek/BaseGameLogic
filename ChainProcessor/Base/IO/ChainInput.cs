using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class ChainInput : IOChainLink 
	{
		private const string Link_Name = "ChainInput";
		public override string Name { get { return Link_Name; } }

		public ChainData InData = null;

		protected override int InputsCount { get { return 0;} }

		public ChainInput (Vector2 position) : base (position) {}

		public override void Prosess ()
		{
			OutData = InData;
			for (int i = 0; i < _outputs.Count; i++) 
			{
//				_outputs [i].OutData = InData;
				_outputs [i].Prosess ();
			}
		}
	}
}