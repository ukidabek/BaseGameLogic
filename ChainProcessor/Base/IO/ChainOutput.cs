using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class ChainOutput : ChainLink 
	{
		public ChainData OutData = null;

		private ChainLink _input = null; 
		public override ChainLink[] Inputs { get { return new ChainLink[]{_input}; }}

		public override ChainLink[] Outputs { get { return null;} }

		public override void Prosess (ChainData data)
		{
			OutData = data;
		}
	}
}