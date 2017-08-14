using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.Events
{
	[CreateAssetMenu(menuName = "Events/EventThrowerConfig", fileName = "EventThrowerConfig.asset")]
	public class EventThrowerConfig : ScriptableObject 
	{
	    [SerializeField]
        private List<EventToThrow> _eventsToThrow = new List<EventToThrow>();	    
	    public List<EventToThrow> EventsToThrow {
	        get { return this._eventsToThrow; }
	    }
	}

	[Serializable]
	public class EventToThrow
	{
        private EventManager EventManagerInstance
        {
            get { return EventManager.Instance; }
        }

        [SerializeField]
	    public string name = string.Empty;

	    [SerializeField] 
	    public string EventID = string.Empty;
	   	
	    public void Throw()
	    {
            if (EventManagerInstance != null)
            {
                EventManagerInstance.FireEvent(name, null);
            }
	    }
	}
}
