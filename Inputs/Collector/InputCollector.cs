using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Inputs
{
    public class InputCollector : MonoBehaviour 
    {
		[SerializeField,Range(0,7)]
		private int _playerNumber = 0;
		public int PlayerNumber 
		{
    		get { return this._playerNumber; }
    	}

        [Header("Debug display & options.")]
		[SerializeField]
        public InputSourceEnum currentInputSourceEnum = InputSourceEnum.Null;
		public Action InputSourceInstanceChanged = null;

		private BaseInputSource _currentInputSourceInstance = null;
		public BaseInputSource CurrentInputSourceInstance 
		{
			get { return _currentInputSourceInstance; }
			protected set 
			{
				if (_currentInputSourceInstance != value) 
				{
					_currentInputSourceInstance = value;
					if (InputSourceInstanceChanged != null) 
					{
						InputSourceInstanceChanged ();
					}
				}
			}
		}

		[SerializeField]
        public GameObject inputSourcesContainerObject = null;

		[Header("Input sources managment.")]
        [SerializeField]
        private List<BaseInputSource> inputSources = new List<BaseInputSource>();
        public List<BaseInputSource> InputSources 
		{ 
            get { return this.inputSources; } 
        }

        public T GetInputSources<T>() where T :BaseInputSource
        {
            foreach (BaseInputSource inputSource in this.inputSources)
            {
                if (inputSource is T)
                {
                    return inputSource as T;
                }
            }

            return null;
        }

		public List<T> GetPhysicalInput<T>(string name) where T:PhysicalInput
		{
			List<T> inputsList = new List<T>();
			foreach (BaseInputSource soure in inputSources)
			{
				if (soure != null) 
				{
					if (soure.GetPhysicalInput<T>(name) as T != null)
					{
						inputsList.Add(soure.GetPhysicalInput<T>(name) as T );
					}
				}
			}

			return inputsList;
		}
			
		public ButtonStateEnum GetKeyState (string ButtonName)
		{
			List<ButtonInput> TmpButton = new List<ButtonInput>(); //only for init

			ButtonStateEnum ButtonState = ButtonStateEnum.Released;

			List<ButtonInput> KeyList = GetPhysicalInput<ButtonInput> (ButtonName);

			for (int i = 0; i < KeyList.Count; i++) 
			{
				ButtonInput buttonInput = KeyList [i];
				if (buttonInput.InputName == ButtonName) 
				{
					TmpButton.Add(buttonInput);
				}
			}

			for (int i = 0; i < TmpButton.Count; i++) 
			{
				ButtonInput buttonInput = TmpButton [i];
				if (buttonInput.State != ButtonStateEnum.Released)
				{
					ButtonState = buttonInput.State;
				}
			}

			return ButtonState;
		}

		public Vector2 ReadAxisFromImput (string AxisName)
		{
			AnalogInput TmpAnalog = null; //only for init

			List<AnalogInput> KeyList = GetPhysicalInput<AnalogInput> (AxisName);
			for (int i = 0; i < KeyList.Count; i++) 
			{
				AnalogInput analogInput = KeyList [i];
				if (analogInput.InputName == AxisName ) 
				{
					TmpAnalog = analogInput;
				}
			}

			return TmpAnalog.Axis;
		}

		public AnalogInput GetAxisImput (string AxisName)
		{
			AnalogInput Axis = GetPhysicalInput<AnalogInput> (AxisName)[0];

			return Axis;
		}

		public BaseInputSource GetCurrentSource ()
		{
			for (int i = 0; i < inputSources.Count; i++) 
			{
				BaseInputSource inputSource = inputSources[i];
				if (inputSource.InputSourceType == currentInputSourceEnum) 
				{
					return inputSource;
				}
			}

			return null;
		}
			
		public bool IsPadConneted ()
		{
			if (Input.GetJoystickNames().Length == 0 || 
				Input.GetJoystickNames()[0] == "")
			{
				return false;
			}
			return true;
		}

		/// <summary>
		/// Selects the current input source instance.
		/// </summary>
		/// <returns><c>true</c>, if input source instance was change, <c>false</c> if the source is the same.</returns>
		/// <param name="source">Source.</param>
		protected virtual bool SelectCurrentInputSourceInstance(BaseInputSource source)
		{
            if (source == CurrentInputSourceInstance && 
                CurrentInputSourceInstance != null && 
                source != null) 
			{
				return false;
			}

			currentInputSourceEnum = source.InputSourceType;
			CurrentInputSourceInstance = source;

			return true;
		}

		public void CollectInputs()
	    {
	    	foreach (BaseInputSource source in inputSources)
			{
				if (source != null) 
				{
					source.GatherInputs ();
					if (source.PositiveReading)
					{
						SelectCurrentInputSourceInstance (source);
					}
				}
			}
		}

		public void CheckInpunts ()
		{
			for (int i = 0; i < InputSources.Count; i++) 
			{
				BaseInputSource source = InputSources [i];
				int sourcePhysicalInputsCount = source.PhysicalInputs.Count;
				for (int j = 0; j < sourcePhysicalInputsCount; j++) 
				{
					PhysicalInput input = source.PhysicalInputs [j];
					if (input is ButtonInput) 
					{
						ButtonInput tmpInputContainer = input as ButtonInput;
						if (tmpInputContainer.keyCode == KeyCode.None) 
						{
							Debug.LogErrorFormat (
								"Input is not assigned to KeyCode!: Input Name {0} {1}", 
								source.GetType(),
								input.InputName);
						}
					}
				}
			}
		}

		private void HandleGamePause()
		{
			if (CurrentInputSourceInstance == null)
				return; 
			
			bool enablePause = CurrentInputSourceInstance.PauseButtonDown;
			bool gameManagerExist = GameManager.Instance != null;
			bool gameIsPlaing = GameManager.Instance.GameStatus == GameStatusEnum.Play;

			if (enablePause && gameManagerExist && gameIsPlaing) 
			{
				GameManager.Instance.PauseGame ();
			}

			if (enablePause && gameManagerExist && !gameIsPlaing) 
			{
				GameManager.Instance.ResumeGame ();
			}
		}

		protected virtual void Awake()
		{
			for (int i = 0; i < InputSources.Count; i++) 
			{
				InputSources [i].Owner = this;
			}

			// Furthermore we make sure that we don't destroy between scenes (this is optional)
            if (inputSources.Count > 0)
            {
                SelectCurrentInputSourceInstance(InputSources[0]);
            }
            else
            {
                // Exeption! 
                Debug.LogError("No input sources! Add input sources!");
            }
		}
			
        public void Update()
        {
			CollectInputs ();
			HandleGamePause ();
        }
    }
}