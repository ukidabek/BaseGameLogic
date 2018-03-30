
using System;

namespace UnityEngine.Events
{
	[Serializable] public class FloatUnityEvent : UnityEvent<float> {}
	
	[Serializable] public class IntUnityEvent : UnityEvent<int> {}

	[Serializable] public class BoolUnityEvent : UnityEvent<bool> {}
}
