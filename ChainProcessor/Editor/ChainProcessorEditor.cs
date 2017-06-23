using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class ChainProcessorEditor : EditorWindow
	{
		private ChainProcessor _processor = null;
		public ChainProcessor Processor 
		{
			get { return this._processor; }
			set { _processor = value; }
		}

//		public ChainProcessorEditor (ChainProcessor _processor)
//		{
//			this._processor = _processor;
//		}
		
	}
}