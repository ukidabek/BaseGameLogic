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

		public override ChainLink[] Inputs  { get { return null;} }


		public ChainInput (Vector2 position) : base (position) {}

		public override void Prosess ()
		{
		}
	}
}