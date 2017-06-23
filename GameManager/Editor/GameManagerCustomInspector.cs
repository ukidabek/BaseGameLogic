using UnityEngine;
using UnityEditor;

using System.Collections;

using BaseGameLogic.Management;

namespace BaseGameLogic
{
	[CustomEditor(typeof(GameManager), true)]
	public class GameManagerCustomInspector : Editor 
	{
		private GameManager _gameManagerInstance = null;

		private void OnEnable()
		{
			if (target is GameManager) 
			{
				_gameManagerInstance = target as GameManager;
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