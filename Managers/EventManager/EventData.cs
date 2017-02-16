using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;


namespace BaseGameLogic.Events
{
	[Serializable]
	public class EventData
	{
		[SerializeField]
		private string _eventDataID = "";
		public string EventDataID {
			get { return this._eventDataID; }
			set { _eventDataID = value; }
		}

		[SerializeField]
		private EventDataTypeEnum _type;
		public EventDataTypeEnum Type {
			get { return this._type; }
			set { _type = value; }
		}

		[SerializeField]
		private int _intValue = 0;
		public int IntValue {
			get { return this._intValue; }
			set { _intValue = value; }
		}

		[SerializeField]
		private float _flaotValue = 0f;
		public float FlaotValue {
			get { return this._flaotValue; }
			set { _flaotValue = value; }
		}

		[SerializeField]
		private bool _boolValue = false;
		public bool BoolValue {
			get { return this._boolValue; } 
			set { _boolValue = value; }
		}

		[SerializeField]
		private string _stringValue = "";
		public string StringValue {
			get { return this._stringValue; }
			set { _stringValue = value; }
		}

		[SerializeField]
		private GameObject _gameObjectValue = null;
		public GameObject GameObjectValue {
			get { return this._gameObjectValue; }
			set { _gameObjectValue = value; }
		}

		[SerializeField]
		private Transform _transfomValue = null;
		public Transform TransfomValue {
			get { return this._transfomValue; }
			set { _transfomValue = value; }
		}

		#if UNITY_EDITOR

		public static void ShowDataSetGui(ref EventData data)
		{
			EditorGUILayout.BeginHorizontal();
			{
				GUILayoutOption[] eventDataIDLabelOptionsSet = { GUILayout.MaxWidth(100f), GUILayout.MinWidth(50.0f) };
				EditorGUILayout.LabelField ("Data ID: ", eventDataIDLabelOptionsSet);
				GUILayoutOption[] eventDataIDTextFieldOptionsSet = { GUILayout.MaxWidth(150f), GUILayout.MinWidth(50.0f) };
				data.EventDataID = EditorGUILayout.TextField (data.EventDataID, eventDataIDTextFieldOptionsSet);

				data.Type = (EventDataTypeEnum)EditorGUILayout.EnumPopup (data.Type);

				switch (data.Type) 
				{
				case EventDataTypeEnum.Int:
					data.IntValue = EditorGUILayout.IntField (data.IntValue);
					break;

				case EventDataTypeEnum.Float:
					data.FlaotValue = EditorGUILayout.FloatField (data.FlaotValue);
					break;

				case EventDataTypeEnum.Bool:
					data.BoolValue = EditorGUILayout.Toggle (data.BoolValue);
					break;

				case EventDataTypeEnum.String:
					data.StringValue = EditorGUILayout.TextField (data.StringValue);
					break;

				case EventDataTypeEnum.GameObject:
					data.GameObjectValue = (GameObject)EditorGUILayout.ObjectField (data.GameObjectValue, typeof(GameObject));
					break;

				case EventDataTypeEnum.Transform:
					data.TransfomValue = (Transform)EditorGUILayout.ObjectField (data.TransfomValue, typeof(Transform));
					break;
				}
			}
			EditorGUILayout.EndHorizontal ();
		}

		#endif
	}
}