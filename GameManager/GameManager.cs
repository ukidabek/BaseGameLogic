using UnityEngine;

using System;
using System.Collections;

using BaseGameLogic.Inputs;
using BaseGameLogic.TimeManagment;
using BaseGameLogic.Events;
using BaseGameLogic.Character;

namespace BaseGameLogic.Management
{
	public class GameManager : MonoBehaviour 
	{
		public static GameManager Instance  { get; private set; }

		public Action ObjectInitializationCallBack = null;

		[SerializeField]
		private GameStatusEnum _gameStatus = GameStatusEnum.Play;
		public GameStatusEnum GameStatus { get { return this._gameStatus; } }

		[SerializeField]
		private GameObject inputCollectorManagerPrefab = null;
		public InputCollectorManager InputCollectorManager { get; protected set; }

		[SerializeField]
		private GameObject characterRegisterPrefab = null;
		public CharacterRegister CharacterRegisterInstance { get; protected set; }

		[SerializeField]
		private GameObject timeManagerPrefab = null;
		public TimeManager TimeManagerInstance { get; protected set; }

        [Obsolete("EventManager is now separate singleton. Use directly singleton reference.")]
        public EventManager EventManagerInstance { get { return EventManager.Instance; } }

		protected virtual void CreateInstance()
		{
			if (Instance == null) 
			{
				Instance = this;
			} 
			else 
			{
				Destroy (this.gameObject);
			}
		}

		protected virtual void CreateManagersInstance()
		{
			InputCollectorManager = CreateInstance<InputCollectorManager> (inputCollectorManagerPrefab);
			CharacterRegisterInstance = CreateInstance<CharacterRegister>(characterRegisterPrefab);
			TimeManagerInstance = CreateInstance<TimeManager>(timeManagerPrefab);
		}

		protected virtual void InitalizeOtherObjects()
		{
			if (ObjectInitializationCallBack != null)
			{
				ObjectInitializationCallBack ();
			}
		}

		protected T CreateInstance<T>(GameObject prefab) where T: MonoBehaviour
		{
			if (prefab == null) 
			{
				// Throw exeption. 
				return null;
			}

			GameObject instance = GameObject.Instantiate (prefab);
			instance.transform.SetParent (this.transform);
			instance.transform.localPosition = Vector3.zero;
			instance.transform.localRotation = Quaternion.identity;

			T componentInstance = instance.GetComponent<T> ();
			return componentInstance;
		}

		protected virtual void Awake()
		{
			CreateInstance ();
			CreateManagersInstance ();
		}

		protected virtual void Update()
		{
			this.enabled = false;
			InitalizeOtherObjects ();
		}

		public void PauseGame()
		{
			_gameStatus = GameStatusEnum.Pause;
			TimeManagerInstance.Factor = 0f;
		}

		public void ResumeGame()
		{
			_gameStatus = GameStatusEnum.Play;
			TimeManagerInstance.Factor = 1f;
		}
	}
}
