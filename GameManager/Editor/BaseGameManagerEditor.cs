using UnityEngine;
using UnityEditor;

using System.Collections.Generic;
using System.Reflection;
using System.Linq;

using BaseGameLogic.Management;

namespace BaseGameLogic
{
	[CustomEditor(typeof(BaseGameManager), true)]
	public class BaseGameManagerEditor : Editor 
	{
		private List<FieldInfo> _managerAttribute = new List<FieldInfo>();
		
        [MenuItem("BaseGameLogic/GameManager")]
        public static BaseGameManager CreateInputCollectorManager()
        {
            return GameObjectExtension.CreateInstanceOfAbstractType<BaseGameManager>();
        }

        private BaseGameManager _gameManagerInstance = null;

		private void OnEnable()
		{
			_gameManagerInstance = target as BaseGameManager;

			_managerAttribute.AddRange(AssemblyExtension.GetAllFieldsWithAttribute(_gameManagerInstance.GetType(), typeof(ManagerAttribute), true));
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			if (Application.isPlaying) 
			{
				if (GUILayout.Button ("Pause")) 
				{
					_gameManagerInstance.PauseGame ();
				}

				if (GUILayout.Button ("Play")) 
				{
					_gameManagerInstance.ResumeGame ();
				}
			}

			foreach (FieldInfo item in _managerAttribute)
			{
				ManagerAttribute managerAttribute = item.GetCustomAttributes(false).First(attribute => attribute.GetType() == typeof(ManagerAttribute)) as ManagerAttribute;
				if(item.GetValue(_gameManagerInstance).Equals(null))
				{
					if(managerAttribute.IsNecessary)
					{
						EditorGUILayout.HelpBox(string.Format("Manager of type {0} doesn't exist and it necessary!", managerAttribute.ManagerType.Name), MessageType.Error);
					}
					else
					{
						EditorGUILayout.HelpBox(string.Format("Manager of type {0} doesn't exist. Some feathers wont work.", managerAttribute.ManagerType.Name), MessageType.Warning);
					}
				}
			}
		}
	}
}