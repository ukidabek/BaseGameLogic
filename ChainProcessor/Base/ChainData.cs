using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public abstract class ChainData 
	{
		#if UNITY_EDITOR

		public abstract void OnInspektorGui();

		#endif
	}
}