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


			GUI.enabled = _processor.LinksFactory != null;
			if (GUILayout.Button ("Open editor")) 
			{
				ChainProcessorEditor window = EditorWindow.CreateInstance<ChainProcessorEditor> ();
				window.Initialize (_processor);
				window.Show ();
			}
			GUI.enabled = true;

			if (GUILayout.Button ("Open editor")) 
			{
//				string xx = string.Empty;
//
//				for (int i = 0; i < _processor.LinkList.Count; i++) 
//				{
//					string a = _processor.LinkList [i].Name + "|";
//
//					for (int j = 0; j < _processor.LinkList [i].Inputs.Length; j++) 
//					{
//						if (_processor.LinkList [i].Inputs [j] == null)
//							continue;
//						int z = _processor.LinkList.IndexOf (_processor.LinkList [i].Inputs [j]);
//						a += z.ToString ();
//
//						if (j < _processor.LinkList [i].Inputs.Length - 1)
//							a += ",";
//					}
//
//					a += "|";
//
//					for (int j = 0; j < _processor.LinkList [i].Outputs.Count; j++) 
//					{						
//						int z = _processor.LinkList.IndexOf (_processor.LinkList [i].Outputs [j]);
//						a += z.ToString ();
//
//						if (j < _processor.LinkList [i].Outputs.Count - 1)
//							a += ",";
//					}
//					a += ";";
//					xx += a;
//				}
//				Debug.Log (xx);
//				_processor.LinkList [3].OutData;
//				Type t = Type.GetType ("BaseGameLogic.ChainProcessing.ChainLink");
//
//				_processor.LinkList [3].CheackOutputType (t, 0);
				ChainData aaa = new MathChainData(5f);
//				Type a = GetType (aaa);
				_processor.LinkList [3].CheackConnectingOutputType (aaa.GetType(), 0);
			}
		}
	}
}