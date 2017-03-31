using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Inputs;

namespace BaseGameLogic.Character
{
	public class BasePlayerCharacterController : BaseCharacterController, IComparable 
	{
		public override bool IsPlayer { get { return true; } }

		[SerializeField]
		protected InputCollector inputCollector = null;
		public InputCollector InputCollector 
		{
			get { return this.inputCollector; }
			protected set { inputCollector = value; }
		}

		protected BaseInputSource CurrentInputSource
		{
			get 
			{ 
				if (InputCollector == null)
					return null;

				return InputCollector.CurrentInputSourceInstance;
			}
		}

		/// <summary>
		/// The input collector.
		/// Contains data on inputs.
		/// </summary>
		protected InputCollectorManager InputCollectorManagerInstance
		{
			get { return GameManagerInstance.InputCollectorManager; }
		}

		[SerializeField, Range(0,7)]
		private int _playerNumber = 0;
		public int PlayerNumber 
		{
			get { return this._playerNumber; }
		}

		public int CompareTo (object obj)
		{
			BasePlayerCharacterController otherPlayer = obj as BasePlayerCharacterController;

			if (_playerNumber < otherPlayer._playerNumber) 
			{
				return 1;
			}

			if (_playerNumber > otherPlayer._playerNumber) 
			{
				return -1;
			}

			return 0;
		}

		protected override void Start ()
		{
			base.Start ();

			InputCollector = InputCollectorManagerInstance.GetInputCollector (PlayerNumber);
		}
	}
}