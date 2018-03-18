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

		/// <summary>
		/// The input collector.
		/// Contains data on inputs.
		/// </summary>
		protected BaseInputCollectorManager InputCollectorManagerInstance
		{
			get { return GameManagerInstance.InputCollectorManager; }
		}

        [SerializeField, Range(0, 8)]
		private int _playerNumber = 0;
		public int PlayerNumber 
		{
			get { return this._playerNumber; }
            set { this._playerNumber = value; }
		}

		public int CompareTo (object obj)
		{
			BasePlayerCharacterController otherPlayer = obj as BasePlayerCharacterController;

			if (PlayerNumber < otherPlayer.PlayerNumber)
				return 1;

			if (PlayerNumber > otherPlayer.PlayerNumber)
				return -1;

			return 0;
		}
    }
}