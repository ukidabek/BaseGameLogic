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
	public abstract class IOChainLink : ChainLink 
	{
		public IOChainLink (Vector2 position) : base (position) {}
	}
}