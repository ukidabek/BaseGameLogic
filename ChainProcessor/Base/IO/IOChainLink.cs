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
		[SerializeField]
		protected string ioName = string.Empty;
		public string IOName 
		{
			get { return this.ioName; }
			set { ioName = value; }
		}

		public IOChainLink (Vector2 position) : base (position) {}

		#if UNITY_EDITOR
		public override void DrawNodeWindow (int id)
		{
			ioName = EditorGUILayout.TextField (ioName);
			base.DrawNodeWindow (id);
		}

		public virtual void DrawIOInspectorGUI()
		{
			EditorGUILayout.BeginHorizontal ();
			{
				EditorGUILayout.LabelField ("Name: ", GUILayout.Width (40));
				ioName = EditorGUILayout.TextField (ioName);
				if (Data != null) 
				{
					Data.DrawDataOnInspektorGUI ();
				}
			}
			EditorGUILayout.EndHorizontal ();
		}
		#endif
	}
}