using UnityEngine;

using System;
using System.Collections;

using BaseGameLogic.Singleton;
using BaseGameLogic.Inputs;
using BaseGameLogic.TimeManagement;
using BaseGameLogic.Character;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine.Events;

namespace BaseGameLogic.Management
{
	public abstract class BaseGameManager : Singleton<BaseGameManager> 
	{
		public InitializeObjects ObjectInitializationCallBack = new InitializeObjects();

		[SerializeField]
		private GameStatusEnum _gameStatus = GameStatusEnum.Play;
		public GameStatusEnum GameStatus { get { return this._gameStatus; } }

		[SerializeField, Manager(true, typeof(BaseInputCollectorManager))]
		private GameObject inputCollectorManagerPrefab = null;
        public GameObject InputCollectorManagerPrefab { get { return inputCollectorManagerPrefab; } }
		public BaseInputCollectorManager InputCollectorManager { get { return BaseInputCollectorManager.Instance; } }

        [SerializeField, Manager(false, typeof(BaseCharacterRegister))]
		private GameObject characterRegisterPrefab = null;
        public GameObject CharacterRegisterPrefab { get { return characterRegisterPrefab; } }
		public BaseCharacterRegister CharacterRegisterInstance { get { return BaseCharacterRegister.Instance; } }

        [SerializeField, Manager(true, typeof(BaseTimeManager))]
		private GameObject timeManagerPrefab = null;
        public GameObject TimeManagerPrefab { get { return timeManagerPrefab; } }
		public BaseTimeManager TimeManagerInstance { get { return BaseTimeManager.Instance; } }


        protected virtual void CreateManagersInstance()
		{
            List<FieldInfo> managersPrefabFields = AssemblyExtension.GetAllFieldsWithAttribute(this.GetType(), typeof(ManagerAttribute), true);
            foreach (FieldInfo managerPrefabField in managersPrefabFields)
			{
				object managerObject = managerPrefabField.GetValue(this);
				if(!managerObject.Equals(null))
					(managerPrefabField.GetValue(this) as GameObject).CreateInstance(transform);
			}
		}

		protected virtual void InitializeOtherObjects()
		{
			ObjectInitializationCallBack.Invoke (this);
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
			this.enabled = false;
            if(_gameStatus != GameStatusEnum.Loading)
            {
			    InitializeOtherObjects ();
            }
		}

		public void PauseGame()
		{
			_gameStatus = GameStatusEnum.Pause;
            if(TimeManagerInstance != null)
            {
			    TimeManagerInstance.Factor = 0f;
            }

            Cursor.visible = true;
        }

        public void ResumeGame()
		{
			_gameStatus = GameStatusEnum.Play;
            if (TimeManagerInstance != null)
            {
                TimeManagerInstance.Factor = 1f;
            }

            Cursor.visible = false;
        }
    }

	[Serializable] public class InitializeObjects : UnityEvent<BaseGameManager> {}
}
