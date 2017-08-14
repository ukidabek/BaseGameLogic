using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Inputs
{
    /// <summary>
    /// Base Input source class. Classes with will extend that class will be used for handling
    /// a input device like kay board, game pad or mouse.
    /// </summary>
	public class BaseInputSource : MonoBehaviour
	{
		public virtual string InputSourceType 
		{
			get { throw new NotImplementedException (); }
		}

		[SerializeField]
        [Tooltip("The reference to input collector that collect input from this source.")]
		private InputCollector _owner = null;
        /// <summary>
        /// The reference to input collector that collect input from this source.
        /// </summary>
		public InputCollector Owner 
		{ 
			get { return _owner; } 
			set { _owner = value; } 
		}

        /// <summary>
        /// List of all PhysicalInputs  (buttons, analog values, ect.) that will be checked when this InputSource is collecting it’s inputs.
        /// </summary>
		protected List<PhysicalInput> physicalInputs = new List<PhysicalInput> ();
        /// <summary>
        /// List of all PhysicalInputs  (buttons, analog values, ect.) that will be checked when this InputSource is collecting it’s inputs.
        /// </summary>
        public List<PhysicalInput> PhysicalInputs 
		{
			get { return physicalInputs; }
		}

        /// <summary>
        /// Return true if only one PhysicalInput is positive. For example button is pleased or analog value is greater then 0 or less.
        /// </summary>
		public bool PositiveReading 
		{
			get 
			{
				foreach (PhysicalInput input in physicalInputs) 
				{
					if (input.PositiveReading)
						return true;
				}

				return false;
			} 
		}

        /// <summary>
        /// Vector used for controlling player motion (WASD, left analog on game pad).
        /// </summary>
		public virtual Vector3 MovementVector 
		{
			get { return Vector3.zero; }
		}

        /// <summary>
        /// Vector containing a analog valeuse (greater then 0 or less) reader from game pad triggers.
        /// </summary>
		public virtual Vector3 TriggersVector
		{
			get { return Vector3.zero; }
		}

		[Header("Look axis sensitivity settings")]
		[SerializeField]
        [Tooltip("Sensitivity of x look axis.")]
		public float xLookAxisSensitivity = 3f;
		[SerializeField]
        [Tooltip("Sensitivity of y look axis.")]
		public float yLookAxisSensitivity = 3f;

        /// <summary>
        /// Vector used for controlling camera motion (mouse, right analog on game pad).
        /// </summary>
        public virtual Vector3 LookVector 
		{
			get { return Vector3.zero; }
		}

        [SerializeField]
        private ButtonInput _pauseButton = new ButtonInput();

        /// <summary>
        /// Is true if the pause button for this InputSource is pressed. 
        /// </summary>
		public virtual bool PauseButtonDown
		{
			get { return _pauseButton.Pressed; }
		}

		protected virtual void Awake ()
		{
			if (physicalInputs.Count == 0) 
			{
				Debug.LogError ("No input detected. Add input source and set inputs");
			}

            physicalInputs.Add(_pauseButton);

            for (int i = 0; i < physicalInputs.Count; i++)
            {
                PhysicalInput input = physicalInputs[i];
                input.SetOwner(this);
            }
		}

		protected virtual void Start()
		{
			if (!Owner.InputSources.Contains (this)) 
			{
				if (this != null) 
				{
					Owner.InputSources.Add (this);
				} 
			}
		}

        /// <summary>
        /// Rad all PhysicalInputs in InputSource.
        /// </summary>
        public virtual void ReadInputs ()
		{
            for (int i = 0; i < physicalInputs.Count; i++)
            {
                PhysicalInput input = physicalInputs[i];
                input.Read();
            }
		}

        /// <summary>
        /// Return the PhysicalInput of type T by it’s name.
        /// </summary>
        /// <typeparam name="T">Type extending PhysicalInput.</typeparam>
        /// <param name="name">Name of input</param>
        /// <returns>PhysicalInput</returns>
		public T GetPhysicalInput<T> (string name) where T:PhysicalInput
		{
			T _input = null;
			foreach (PhysicalInput input in physicalInputs) 
			{
				if (input is T && input.InputName.Equals (name))
				{
					_input = input as T;
					break;
				}
			}

			if (_input == null)
				throw new NoPhysicalInputException(name, typeof(T));

			return _input;
		}	                           
	}
}