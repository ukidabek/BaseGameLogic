using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Character
{
	public class BasePlayerCharacterController : BaseCharacterController, IComparable 
	{
		public override bool IsPlayer { get { return true; } }

		[SerializeField]
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
	}
}