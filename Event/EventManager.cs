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
        private const string Anather_Instance_Error = "There is another EventManager instance. Please delete it!";

		#if UNITY_EDITOR

        /// <summary>
        /// Enable or disable log of EventManager.
        /// </summary>
		[SerializeField]
		private bool showLogData = false;

		#endif

        /// <summary>
        /// EventManager instance. 
        /// </summary>
        public static EventManager Instance { get; private set; }

        /// <summary>
        /// EventManager existence check 
        /// </summary>
        public static bool EventCanBeRegistred { get { return Instance != null; } }

        /// <summary>
        /// The dictionary of event.
        /// </summary>
        private Dictionary<string, Action<object>> _eventDictionary = new Dictionary<string, Action<object>>();
		public Dictionary<string, Action<object>> EventDictionary
        {
			get{ return _eventDictionary; }
		}

        private void Awake()
        {
            if(Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Debug.LogError(Anather_Instance_Error);
            }
        }


        /// <summary>
        /// Creates null event of ID.
        /// </summary>
        /// <param name="eventID">Event ID.</param>
        public void CreateEvent(string eventID)
		{
			Action<object> eventAction = null;
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
			Action<object> eventAction = null;
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
		public void RegisterEventAction(string eventID, Action<object> action)
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
		public void UnregisterEventAction(string eventID, Action<object> action = null)
		{
			Action<object> eventAction = null;
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
		/// <param name="eventData">Reference to EventClient object.</param>
		public void FireEvent(string eventID, object eventData)
		{
			Action<object> eventAction = null;
			bool containValue = false;

			containValue = _eventDictionary.TryGetValue (eventID, out eventAction);
			if (containValue && eventAction != null) 
			{
				eventAction (eventData);
			}
		}
	}
}
