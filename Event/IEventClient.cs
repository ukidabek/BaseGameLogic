using UnityEngine;

using System.Collections;

using BaseGameLogic;

namespace BaseGameLogic.Events
{
	public interface IEventClient  
	{
		EventManager EventManagerInstance { get; }

		void EventClientStart(); 

		void EventClientOnDestroy ();

		/// <summary>
		/// Registars all events of object. Called on Start().
		/// </summary>
		void RegisterAllEvents();

		/// <summary>
		/// Unregisters all events of object. Called on OnDestroy().
		/// </summary>
		void UnregisterAllEvents();
	}
}
