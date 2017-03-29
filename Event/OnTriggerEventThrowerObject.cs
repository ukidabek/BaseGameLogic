using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Events
{
	public class OnTriggerEventThrowerObject : MonoBehaviour, IEventClient 
	{
		public EventManager EventManagerInstance 
		{
			get { return GameManager.Instance.EventManagerInstance; }
		}

		private Collider objectCollider = null;

		[SerializeField]
		private string _onTriggerEnterEvent = "";
		[SerializeField]
		private bool _disableAfterTriggerEnter = false;

		[SerializeField]
		private string _onTriggerExitEvent = "";
		[SerializeField]
		private bool _disableAfterTriggerExit = false;

		[SerializeField, HideInInspector]
		private EventDataContainer _eventDataContainerOnEnter = new EventDataContainer();
		public EventDataContainer EventDataContainerOnEnter 
		{
			get { return this._eventDataContainerOnEnter; }
		}

		[SerializeField, HideInInspector]
		private EventDataContainer _eventDataContainerOnExit = new EventDataContainer();
		public EventDataContainer EventDataContainerOnExit 
		{
			get { return this._eventDataContainerOnExit; }
		}

		public void EventClientStart () {}

		public void EventClientOnDestroy () {}

		public void RegisterAllEvents () {}

		public void UnregisterAllEvents () {}

		private void Awake()
		{
			objectCollider = GetComponent<Collider> ();
			if (objectCollider != null) 
			{
				objectCollider.isTrigger = true;
			}

			EventDataContainerOnEnter.Initialization ();
		}

		private void Disable(bool value)
		{
			if (this.gameObject.activeSelf != value) 
			{
				this.gameObject.SetActive (value);
			}
		}

		private void OnTriggerEnter(Collider other) 
		{
			Disable (_disableAfterTriggerEnter);

			if (EventManagerInstance == null)
				return;

			if (!_onTriggerEnterEvent.Equals ("")) 
			{
				string eventId = _onTriggerEnterEvent;
				EventManagerInstance.FireEvent (eventId, this);
			}
		}

		private void OnTriggerExit(Collider other) 
		{
			Disable (_disableAfterTriggerExit);

			if (EventManagerInstance == null)
				return;

			if (!_onTriggerExitEvent.Equals ("")) 
			{
				string eventId = _onTriggerExitEvent;
				EventManagerInstance.FireEvent (eventId, this);
			}
		}
	}
}