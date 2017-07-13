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
			_processor.Reprint -= this.Repaint;
			_processor.Reprint += this.Repaint;
		}

		private void Awake()
		{
			OnEnable ();
		}

		private void OnDisable()
		{
			_processor.Reprint -= this.Repaint;
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			EditorGUILayout.BeginVertical ();
			{
				EditorGUILayout.LabelField ("Inputs: ");
				for (int i = 0; i < _processor.Inputs.Count; i++) 
				{
					_processor.Inputs [i].DrawIOInspectorGUI ();
				}
			}
			EditorGUILayout.EndVertical ();

			EditorGUILayout.Space ();

			EditorGUILayout.BeginVertical ();
			{
				EditorGUILayout.LabelField ("Outputs: ");
				for (int i = 0; i < _processor.Outputs.Count; i++) 
				{
					_processor.Outputs [i].DrawIOInspectorGUI ();
				}
			}
			EditorGUILayout.EndVertical ();

			GUI.enabled = _processor.LinksFactory != null;
			if (GUILayout.Button ("Open editor")) 
			{
				ChainProcessorEditor window = EditorWindow.CreateInstance<ChainProcessorEditor> ();
				window.Initialize (_processor);
				window.Show ();
			}
			GUI.enabled = true;
		}
	}
}