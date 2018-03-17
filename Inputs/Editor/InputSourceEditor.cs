using UnityEngine;
using UnityEditor;

using System.Collections;

namespace BaseGameLogic.Inputs
{
	public class InputSourceEditor : EditorWindow
	{
		private Vector2 scrollPosition = Vector2.zero;

		private int inputCollectorInstanceID = 0;
		private int inputSourceIndex = -1;
	        
		private BaseInputSource baseInputSource = null;
		public BaseInputSource BaseInputSource 
		{
			get 
			{
				if (baseInputSource == null) 
				{
					BaseInputCollector[] InputCollectors = GameObject.FindObjectsOfType<BaseInputCollector> ();

					foreach (BaseInputCollector collector in InputCollectors) 
					{
						if (collector.GetInstanceID () == inputCollectorInstanceID) 
						{
							baseInputSource = collector[inputSourceIndex];
						}
					}
				}
					
				return this.baseInputSource;
			}
			set 
			{
				baseInputSource = value;
			}
		}

		private Editor baseInputSourceInspector = null;

		public Editor BaseInputSourceInspector 
		{
			get 
			{
				if (baseInputSource == null || baseInputSourceInspector == null)
				{
					this.baseInputSourceInspector = Editor.CreateEditor (BaseInputSource);
				}
				return this.baseInputSourceInspector;
			}
		}

		public InputSourceEditor (BaseInputCollector inputCollector, int inputSourceIndex)
		{
			this.inputCollectorInstanceID = inputCollector.GetInstanceID ();
			this.inputSourceIndex = inputSourceIndex;
			this.baseInputSource = inputCollector[inputSourceIndex];
			this.Show ();
		}

		public void OnGUI ()
		{
			scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition);
			{
				if(BaseInputSourceInspector != null)
					BaseInputSourceInspector.OnInspectorGUI ();        
			}
			EditorGUILayout.EndScrollView ();
		}
	}
}
