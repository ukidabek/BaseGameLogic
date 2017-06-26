using UnityEngine;
using UnityEditor;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	[CustomEditor(typeof(ChainProcessor))]
	public class ChainProcessorCustomInspector : Editor
	{
		private ChainProcessor _processor = null;

		private void OnEnable()
		{
			_processor = target as ChainProcessor;
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (GUILayout.Button ("Open editor")) 
			{
				ChainProcessorEditor window = EditorWindow.CreateInstance<ChainProcessorEditor> ();
				window.Initialize (_processor);
				window.Show ();
			}
		}
	}
}