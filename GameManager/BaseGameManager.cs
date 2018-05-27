using UnityEngine;
using UnityEngine.Events;

using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Singleton;
using BaseGameLogic.Inputs;
using BaseGameLogic.TimeManagement;

namespace BaseGameLogic.Management
{
	public class BaseGameManager : Singleton<BaseGameManager> 
	{
		public InitializeObjects ObjectInitializationCallBack = new InitializeObjects();

		[SerializeField]
		private GameStatusEnum _gameStatus = GameStatusEnum.Play;
		public GameStatusEnum GameStatus { get { return this._gameStatus; } }

        protected virtual void CreateManagersInstance()
		{
            List<FieldInfo> managersPrefabFields = AssemblyExtension.GetAllFieldsWithAttribute(this.GetType(), typeof(ManagerAttribute), true);
            foreach (FieldInfo managerPrefabField in managersPrefabFields)
			{
				object managerObject = managerPrefabField.GetValue(this);
				GameObject gameObject = managerObject as GameObject;
				if(gameObject != null)
                {
					var instance = gameObject.CreateInstance(transform);
                    IInitialize initialize = instance.GetComponentInChildren<IInitialize>();
                    if(initialize != null) ObjectInitializationCallBack.AddListener(initialize.Initialize);
                }
			}
		}

		protected virtual void InitializeOtherObjects()
		{
			ObjectInitializationCallBack.Invoke (this);
            ObjectInitializationCallBack.RemoveAllListeners();
        }

		protected override void Awake()
		{
            base.Awake();

            transform.ResetPosition();
            transform.ResetRotation();

			CreateManagersInstance ();

            Cursor.visible = false;
        }

		protected virtual void Update()
		{
            if(_gameStatus != GameStatusEnum.Loading)
            {
			    this.enabled = false;
			    InitializeOtherObjects ();
            }
		}

		public virtual void PauseGame()
		{
			_gameStatus = GameStatusEnum.Pause;
        }

        public void LoadGame()
        {
            _gameStatus = GameStatusEnum.Loading;
        }

        public virtual void ResumeGame()
		{
			_gameStatus = GameStatusEnum.Play;
        }
    }

	[Serializable] public class InitializeObjects : UnityEvent<BaseGameManager> {}
}
