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
	[Serializable]
	public abstract class ChainLink
	{
		#if UNITY_EDITOR

		private Rect _linkRect = new Rect ();
		public Rect LinkRect 
		{
			get { return this._linkRect; }
			set { _linkRect = value; }
		}

		#endif

		public abstract ChainLink [] Inputs { get; }
		public abstract ChainLink [] Outputs { get; }

		public abstract void Prosess(ChainData data);

		#if UNITY_EDITOR

		public virtual void DrawNodeWindow(int id) 
		{
			GUI.DragWindow();
		}

		#endif
	}
}