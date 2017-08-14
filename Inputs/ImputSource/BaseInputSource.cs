using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Inputs
{
	public class BaseInputSource : MonoBehaviour
	{
		public virtual string InputSourceType 
		{
			get { throw new NotImplementedException (); }
		}

		[SerializeField]
		private InputCollector _owner = null;
		public InputCollector Owner 
		{ 
			get { return _owner; } 
			set { _owner = value; } 
		}

		protected List<PhysicalInput> physicalInputs = new List<PhysicalInput> ();
		public List<PhysicalInput> PhysicalInputs 
		{
			get { return physicalInputs; }
		}

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

		public virtual Vector3 MovementVector 
		{
			get { return Vector3.zero; }
		}

		public virtual Vector3 TriggersVector
		{
			get { return Vector3.zero; }
		}

		[Header("Look axis sensitivity settings")]
		[SerializeField, Tooltip("Sensitivity of x look axis.")]
		public float xLookAxisSensitivity = 3f;
		[SerializeField, Tooltip("Sensitivity of y look axis.")]
		public float yLookAxisSensitivity = 3f;

		public virtual Vector3 LookVector 
		{
			get { return Vector3.zero; }
		}

		public virtual bool PauseButtonDown
		{
			get { return false; }
		}

		protected virtual void Awake ()
		{
			if (physicalInputs.Count == 0) 
			{
				Debug.LogError ("No input detected. Add input source and set inputs");
			}
				
			foreach (PhysicalInput input in physicalInputs) 
			{
				input.SetOwner (this);
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
			
		public virtual void GatherInputs ()
		{
			foreach (PhysicalInput input in physicalInputs) 
			{
				input.Read ();
			}
		}

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