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
	public abstract class ChainLink : MonoBehaviour
	{
		public abstract string Name { get; }

		public ChainData OutData = null;

		#if UNITY_EDITOR
		[SerializeField]
		protected Rect _linkRect = new Rect ();
		public Rect LinkRect 
		{
			get { return this._linkRect; }
			set { _linkRect = value; }
		}

		#endif

		public abstract ChainLink [] Inputs { get; }
		public abstract ChainLink [] Outputs { get; }

		public abstract void Prosess();

		#if UNITY_EDITOR

		public virtual void DrawNodeWindow(int id) 
		{
			GUI.DragWindow();
		}

		#endif

		public ChainLink (Vector2 position)
		{
			this._linkRect.x = position.x;
			this._linkRect.y = position.y;
		}
		
	}
}