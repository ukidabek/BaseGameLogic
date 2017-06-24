using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class ChainInput : ChainLink 
	{
		public ChainData InData = null;

		public override ChainLink[] Inputs  { get { return null;} }

		private ChainLink _output = null; 
		public override ChainLink[] Outputs 
		{
			get { return new ChainLink[]{_output}; }
		}

		public override void Prosess (ChainData data)
		{
			_output.Prosess (InData);
		}
	}
}