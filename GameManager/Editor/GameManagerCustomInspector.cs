using UnityEngine;
using UnityEditor;

using System.Collections;

using BaseGameLogic.Management;

namespace BaseGameLogic
{
	[CustomEditor(typeof(BaseGameManager), true)]
	public class GameManagerCustomInspector : Editor 
	{
        [MenuItem("BaseGameLogic/GameManager")]
        public static BaseGameManager CreateInputCollectorManager()
        {
            return GameObjectExtension.CreateInstanceOfAbstractType<BaseGameManager>();
        }

        private BaseGameManager _gameManagerInstance = null;

		private void OnEnable()
		{
			if (target is BaseGameManager) 
			{
				_gameManagerInstance = target as BaseGameManager;
			}
		}

		public override void OnInspectorGUI ()
		{
			base.OnInspectorGUI ();

			bool pauseButtonPres = false;
			bool playButtonPres = false;

			pauseButtonPres = GUILayout.Button ("Pause");
			playButtonPres = GUILayout.Button ("Play");

			if (Application.isPlaying) 
			{
				if (pauseButtonPres) 
				{
					_gameManagerInstance.PauseGame ();
				}

				if (playButtonPres) 
				{
					_gameManagerInstance.ResumeGame ();
				}
			}
		}
	}
}