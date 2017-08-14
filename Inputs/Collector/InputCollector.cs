using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.Inputs
{
    public class InputCollector : MonoBehaviour 
    {
        private const string Input_KeyCode_Error_Message = "Input is not assigned to KeyCode!: Input Name {0} {1}";
        private const string No_Input_Sources_Error_Message = "No input sources! Add input sources!";

		[SerializeField]
        [Range(0,7)]
        [Tooltip("Number of player that InputCollector will be assigned do.")]
		private int _playerNumber = 0;
        /// <summary>
        /// If more the one local play is used to bound InputCollector to PlayerController by player number.
        /// </summary>
        public int PlayerNumber 
		{
    		get { return this._playerNumber; }
    	}

        [Header("Debug display & options.")]
        [SerializeField]
        [Tooltip("Current input type name. ")]
        public string currentInputSourceType = string.Empty;
        /// <summary>
        /// Current input type name. 
        /// </summary>
        public string CurrentInputSourceType {  get { return currentInputSourceType; } }

        public Action InputSourceInstanceChanged = null;

		private BaseInputSource _currentInputSourceInstance = null;
        /// <summary>
        /// Current InputSource instance.
        /// </summary>
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

		[Header("Input sources managment.")]
        [SerializeField]
        private List<BaseInputSource> inputSources = new List<BaseInputSource>();
        /// <summary>
        /// List of InputSources from which InputCollector will collect input.
        /// </summary>
        public List<BaseInputSource> InputSources 
		{ 
            get { return this.inputSources; } 
        }

        /// <summary>
        /// Return InputSource by it's type. 
        /// </summary>
        /// <typeparam name="T">Type of InputSource</typeparam>
        /// <returns>InputSource of type T. </returns>
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

        /// <summary>
        /// Returns true if GamePad if connected.
        /// </summary>
        /// <returns></returns>
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

			currentInputSourceType = source.InputSourceType;
			CurrentInputSourceInstance = source;

			return true;
		}

        /// <summary>
        /// Collecting inputs.
        /// </summary>
		public void CollectInputs()
	    {
			for (int i = 0; i < inputSources.Count; i++) 
			{
				BaseInputSource source = inputSources[i];
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

        /// <summary>
        /// Check the correctness of the input configuration.
        /// </summary>
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
								Input_KeyCode_Error_Message, 
								source.GetType(),
								input.InputName);
						}
					}
				}
			}
		}

        /// <summary>
        /// Enable or disable game pause by player input.
        /// </summary>
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

            if (inputSources.Count > 0)
            {
                SelectCurrentInputSourceInstance(InputSources[0]);
            }
            else
            {
                // Exeption! 
                Debug.LogError(No_Input_Sources_Error_Message);
            }
		}
			
        public void Update()
        {
			CollectInputs ();
			HandleGamePause ();
        }
    }
}