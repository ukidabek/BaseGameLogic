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

		protected abstract ChainData Add (ChainData data);
		public static ChainData operator + (ChainData data1, ChainData data2)
		{
			return data1.Add (data2);
		}

		protected abstract ChainData Subtract (ChainData data);
		public static ChainData operator - (ChainData data1, ChainData data2)
		{
			return data1.Subtract (data2);
		}

		protected abstract ChainData Multiply(ChainData data);
		public static ChainData operator * (ChainData data1, ChainData data2)
		{
			return data1.Multiply (data2);
		}

	}
}