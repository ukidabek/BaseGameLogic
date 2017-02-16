using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic;

namespace BaseGameLogic.Events
{
	/// <summary>
	/// Class to menage events.
	/// </summary>
	public class EventManager : MonoBehaviour 
	{
		#if UNITY_EDITOR

		[SerializeField]
		private bool showLogData = true;

		#endif

		/// <summary>
		/// The dictionary of event.
		/// </summary>
		private Dictionary<string, Action<IEventClient>> _eventDictionary = new Dictionary<string, Action<IEventClient>>();
		public Dictionary<string, Action<IEventClient>> EventDictionary {
			get{ return _eventDictionary; }
		}


		/// <summary>
		/// Creates null event of ID.
		/// </summary>
		/// <param name="eventID">Event ID.</param>
		public void CreateEvent(string eventID)
		{
			Action<IEventClient> eventAction = null;
			bool containValue = false;

			containValue = _eventDictionary.TryGetValue (eventID, out eventAction);
			if (!containValue) 
			{
				_eventDictionary.Add (eventID, null);
				#if UNITY_EDITOR
				if(showLogData)
				{
					Debug.LogFormat("Event of id:{0} was created.", eventID);
				}
				#endif
			} 
		}

		/// <summary>
		/// Removes event of ID.
		/// </summary>
		/// <param name="eventID">Event ID.</param>
		public void RemoveEvent(string eventID)
		{
			Action<IEventClient> eventAction = null;
			bool containValue = false;

			containValue = _eventDictionary.TryGetValue (eventID, out eventAction);
			if (containValue) 
			{
				eventAction = null;
				_eventDictionary.Remove (eventID);
			}
		}

		/// <summary>
		/// Registers event of ID.
		/// </summary>
		/// <param name="eventID">Event ID.</param>
		public void RegisterEventAction(string eventID, Action<IEventClient> action)
		{
			bool containValue = false;

			containValue = _eventDictionary.ContainsKey (eventID);
			if (!containValue) 
			{
				_eventDictionary.Add (eventID, action);
				#if UNITY_EDITOR
				if(showLogData)
				{
					Debug.LogFormat("Event of id:{0} was registred & added", eventID);
				}
				#endif
			} 
			else 
			{
				_eventDictionary[eventID] += action;

				#if UNITY_EDITOR
				if(showLogData)
				{
					Debug.LogFormat("Event of id:{0} was registred.", eventID);
				}
				#endif
			}

			foreach (var t in _eventDictionary) {
				Debug.Log (t.Key);
			}

		}

		/// <summary>
		/// Unregisters the event.
		/// </summary>
		/// <param name="eventID">Event ID.</param>
		public void UnregisterEventAction(string eventID, Action<IEventClient> action = null)
		{
			Action<IEventClient> eventAction = null;
			bool containValue = false;

			containValue = _eventDictionary.TryGetValue (eventID, out eventAction);
			if (containValue) 
			{
				eventAction -= action;
				if (eventAction == null)
					_eventDictionary.Remove (eventID);

				#if UNITY_EDITOR
				if(showLogData)
				{
					Debug.LogFormat("Event of id:{0} was unregistred.", eventID);
				}
				#endif
			}
		}
			
		/// <summary>
		/// Fires event of ID.
		/// </summary>
		/// <param name="eventID">Event ID.</param>
		/// <param name="eventClient">Reference to EventClient object.</param>
		public void FireEvent(string eventID, IEventClient eventClient)
		{
			Action<IEventClient> eventAction = null;
			bool containValue = false;

			containValue = _eventDictionary.TryGetValue (eventID, out eventAction);
			if (containValue && eventAction != null) 
			{
				eventAction (eventClient);
			}
		}
	}
}
