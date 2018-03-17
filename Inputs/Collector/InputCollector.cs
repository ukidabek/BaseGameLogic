using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.Inputs
{
    public abstract class InputCollector : MonoBehaviour 
    {
        private const string Input_KeyCode_Error_Message = "Input is not assigned to KeyCode!: Input Name {0} {1}";
        private const string No_Input_Sources_Error_Message = "No input sources! Add input sources!";

		[SerializeField]
        [Range(0,7)]
        [Tooltip("Number of player that InputCollector will be assigned do.")]
		private int _playerNumber = 0;
        /// <summary>
        /// If more the one local play is used to bound InputCollector to HellspawnPlayerController  by player number.
        /// </summary>
        public int PlayerNumber { get { return this._playerNumber; } }

        [Header("Debug display & options.")]
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

        public event Action<BaseInputSource> InputSourceChanged = null;

        [SerializeField, HideInInspector]
        private List<BaseInputSource> inputSources = new List<BaseInputSource>();
        /// <summary>
        /// List of InputSources from which InputCollector will collect input.
        /// </summary>
        // public List<BaseInputSource> InputSources { get { return this.inputSources; } }

        public BaseInputSource this[int i]
        {
            get { return inputSources[i]; }
        }

		public int Count { get { return inputSources.Count; } }

        /// <summary>
        /// Returns true if GamePad if connected.
        /// </summary>
        /// <returns></returns>
		public bool IsPadConnected ()
		{
			if (Input.GetJoystickNames().Length == 0 || string.IsNullOrEmpty(Input.GetJoystickNames()[0]))
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
		protected virtual void SelectCurrentInputSourceInstance(BaseInputSource source)
		{           
            if(CurrentInputSourceInstance != source)
            {
			    CurrentInputSourceInstance = source;
                if(InputSourceChanged != null)
                {
                    InputSourceChanged(source);
                }
            }
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
					source.ReadInputs ();
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
		public void CheckInputs ()
		{
			for (int i = 0; i < Count; i++) 
			{
				for (int j = 0; j < this[i].PhysicalInputs.Count; j++) 
				{
					PhysicalInput input = this[i].PhysicalInputs [j];
					if (input is ButtonInput) 
					{
						if ((input as ButtonInput).keyCode == KeyCode.None) 
						{
							Debug.LogErrorFormat (Input_KeyCode_Error_Message, this[i].GetType(), input.InputName);
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
			bool gameIsPlaying = GameManager.Instance.GameStatus == GameStatusEnum.Play;

			if (enablePause && gameManagerExist && gameIsPlaying) 
			{
				GameManager.Instance.PauseGame ();
			}

			if (enablePause && gameManagerExist && !gameIsPlaying) 
			{
				GameManager.Instance.ResumeGame ();
			}
		}

		protected virtual void Awake()
		{
			for (int i = 0; i < Count; i++) 
			{
                this[i].Owner = this;
			}

            if (inputSources.Count > 0)
            {
                SelectCurrentInputSourceInstance(this[0]);
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

#if UNITY_EDITOR
		private GameObject CreateInputSourceHolder(string name)
		{
            GameObject gameObject = new GameObject();
            gameObject.name = name;
            gameObject.transform.SetParent(this.transform, false);

            return gameObject;
        }
		public void AddInputSource(Type inputSourceType)
		{
            if(inputSourceType.IsSubclassOf(typeof(BaseInputSource)))
			{
				GameObject gameObject = CreateInputSourceHolder(inputSourceType.Name);
				BaseInputSource source = gameObject.AddComponent(inputSourceType) as BaseInputSource;

				inputSources.Add(source);
			}
			else
			{
                throw new TypeIsNotInputSourceException();
            }
        }

		public void AddInputSource<T>() where T : BaseInputSource
		{
            GameObject gameObject = CreateInputSourceHolder(typeof(T).Name);
            T source = gameObject.AddComponent<T>();

            inputSources.Add(source);
        }

		public void RemoveAt(int index)
		{
			if(this[index] != null)
				DestroyImmediate(this[index].gameObject);
            inputSources.RemoveAt(index);
        }
#endif
    }
}