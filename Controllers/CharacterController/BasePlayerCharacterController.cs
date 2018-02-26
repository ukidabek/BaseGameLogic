using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Inputs;

namespace BaseGameLogic.Character
{
	public abstract class BasePlayerCharacterController : BaseCharacterController, IComparable 
	{
		public override bool IsPlayer { get { return true; } }

		//[SerializeField]
		//protected InputCollector inputCollector = null;
		//public InputCollector InputCollector 
		//{
		//	get { return this.inputCollector; }
		//	protected set { inputCollector = value; }
		//}

		/// <summary>
		/// The input collector.
		/// Contains data on inputs.
		/// </summary>
		protected InputCollectorManager InputCollectorManagerInstance
		{
			get { return GameManagerInstance.InputCollectorManager; }
		}

        public int PlayerID = -1;

		[SerializeField]
		private int _playerNumber = 0;
		public int PlayerNumber 
		{
			get { return this._playerNumber; }
            set { this._playerNumber = value; }
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

		}
	}
}