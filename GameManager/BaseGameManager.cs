using UnityEngine;

using System;
using System.Collections;

using BaseGameLogic.Singleton;
using BaseGameLogic.Inputs;
using BaseGameLogic.TimeManagement;
using BaseGameLogic.Events;
using BaseGameLogic.Character;
using System.Reflection;
using System.Collections.Generic;

namespace BaseGameLogic.Management
{
	public abstract class BaseGameManager : Singleton<BaseGameManager> 
	{
		public event Action ObjectInitializationCallBack = null;

		[SerializeField]
		private GameStatusEnum _gameStatus = GameStatusEnum.Play;
		public GameStatusEnum GameStatus { get { return this._gameStatus; } }

		[SerializeField, Manager]
		private GameObject inputCollectorManagerPrefab = null;
		public BaseInputCollectorManager InputCollectorManager { get { return BaseInputCollectorManager.Instance; } }

        [SerializeField, Manager]
		private GameObject characterRegisterPrefab = null;
		public BaseCharacterRegister CharacterRegisterInstance { get { return BaseCharacterRegister.Instance; } }

        [SerializeField, Manager]
		private GameObject timeManagerPrefab = null;
		public BaseTimeManager TimeManagerInstance { get { return BaseTimeManager.Instance; } }

		private List<FieldInfo> GetAllManagerPrefabFields(Type type)
		{
            List<FieldInfo> fields = new List<FieldInfo>();
            if(type == typeof(MonoBehaviour)) return fields;

            fields.AddRange(type.GetAllFieldsWithAttribute<ManagerAttribute>());
            fields.AddRange(GetAllManagerPrefabFields(type.BaseType));

            return fields;
        }

		protected virtual void CreateManagersInstance()
		{
            List<FieldInfo> managersPrefabFields = GetAllManagerPrefabFields(this.GetType());
            foreach (FieldInfo managerPrefabField in managersPrefabFields)
                (managerPrefabField.GetValue(this) as GameObject).CreateInstance(transform);
		}

		protected virtual void InitializeOtherObjects()
		{
			if (ObjectInitializationCallBack != null)
			{
				ObjectInitializationCallBack ();
			}
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
}
