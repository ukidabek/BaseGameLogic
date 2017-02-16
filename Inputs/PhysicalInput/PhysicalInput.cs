using UnityEngine;

using System;
using System.Collections;
namespace BaseGameLogic.Inputs
{
	/// <summary>
	/// Physical input.
	/// </summary>
	[Serializable]
	public abstract class PhysicalInput  
	{
		/// <summary>
		/// The owner reference to a input.
		/// </summary>
		protected BaseInputSource owner = null;
		public BaseInputSource Owner {
			get { return owner; }
		}
		/// <summary>
		/// The name of the input.
		/// </summary>
		[SerializeField]
		private string inputName = "";
		public string InputName 
		{
			get { return this.inputName; }
			set { this.inputName = value; }
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="BaseGameLogic.Inputs.AnalogInput"/> was read.
		/// </summary>
		/// <value><c>true</c> if positive reading; otherwise, <c>false</c>.</value>
		public abstract bool PositiveReading
		{
			get;
		}

		/// <summary>
		/// Sets the owner PhysicalInput.
		/// </summary>
		/// <param name="owner">Owner.</param>
		public void SetOwner(BaseInputSource owner)
		{
			this.owner = owner;
		}

		/// <summary>
		/// It defines how the input is read
		/// </summary>
		public abstract void Read();
	}
}