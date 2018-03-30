
namespace UnityEngine.Events
{
	using Serializable = System.SerializableAttribute;
	using Object = UnityEngine.Object;

	[Serializable] public class FloatUnityEvent : UnityEvent<float> {}
	
	[Serializable] public class IntUnityEvent : UnityEvent<int> {}

	[Serializable] public class BoolUnityEvent : UnityEvent<bool> {}

	[Serializable] public class StringUnityEvent : UnityEvent <string> {}

	[Serializable] public class ObjectUnityEvent : UnityEvent <Object> {}
}
