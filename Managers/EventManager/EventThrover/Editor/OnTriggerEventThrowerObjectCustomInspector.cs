using UnityEngine;
using UnityEditor;

using System.Collections;

namespace BaseGameLogic.Events
{
	[CustomEditor(typeof(OnTriggerEventThrowerObject))]
	public class OnTriggerEventThrowerObjectCustomInspector : Editor
	{
		private OnTriggerEventThrowerObject _onTriggerEventThroverObject;

		private void OnEnable()
		{
			_onTriggerEventThroverObject = target as OnTriggerEventThrowerObject;
		}


		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();
			EventDataContainer eventDataContaincer = _onTriggerEventThroverObject.EventDataContainerOnEnter;
			EventDataContainer.EventDataContainerGUI (ref eventDataContaincer, "On enter event data container");

			eventDataContaincer = _onTriggerEventThroverObject.EventDataContainerOnExit;
			EventDataContainer.EventDataContainerGUI (ref eventDataContaincer, "On exit event data container");
		}
	}
}