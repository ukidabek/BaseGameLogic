using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Events
{
	[Serializable]
	public class EventDataContainer 
	{
		[SerializeField]
		private List<EventData> list = new List<EventData>();
		public List<EventData> List 
		{
			get { return this.list; }
		}

		private Dictionary<string, EventData> eventDataDictionary = new Dictionary<string, EventData>();

		public void Initialization()
		{
			for (int i = 0; i < list.Count; i++) 
			{
				EventData data = list [i];
				if (data != null) 
				{
					eventDataDictionary.Add (data.EventDataID, data);
				}
			}
		}

		public EventData GetEventData(string eventDataID)
		{
			EventData data = null;

			eventDataDictionary.TryGetValue (eventDataID, out data);

			return data;
		}
	
		#if UNITY_EDITOR

		public static void EventDataContainerGUI(ref EventDataContainer eventDataContainer, string label = "")
		{
			if (label != string.Empty) 
			{
				EditorGUILayout.LabelField (label);
			}

			bool addButtonPress =  GUILayout.Button("Add Event data");
			if (addButtonPress) 
			{
				eventDataContainer.List.Add (new EventData ());
			}

			int count = eventDataContainer.List.Count;
			for (int i = 0; i < count; i++) 
			{
				EventData data = eventDataContainer.List [i];

				EditorGUILayout.BeginHorizontal ();
				{
					EventData.ShowDataSetGui (ref data);

					bool removeButtonPress =  GUILayout.Button("-");
					if (removeButtonPress) 
					{
						eventDataContainer.List.RemoveAt (i);
					}
				}
				EditorGUILayout.EndHorizontal ();
			}
		}

		#endif
	}
}