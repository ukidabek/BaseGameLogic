using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Character
{
	public class CharacterRegister : MonoBehaviour 
	{
		private Dictionary<string, BaseCharacterController> charactersDictionary = new Dictionary<string, BaseCharacterController>();

		private List<BasePlayerCharacterController> _playersList = new List<BasePlayerCharacterController>();

		public void RegisterCharacter(BaseCharacterController character)
		{
			bool containsValue = charactersDictionary.ContainsValue (character);
			if (containsValue) 
			{
				//	Exeption
			} 
			else 
			{
				string characterName = character.name;
				charactersDictionary.Add (characterName, character);

				if (character.IsPlayer) 
				{
					BasePlayerCharacterController player = character as BasePlayerCharacterController;
					_playersList.Add (player);
					_playersList.Sort ();
				}
			}
		}

		public void UnregisterCharacter(BaseCharacterController character)
		{
			string characterName = character.name;
			bool valueRemoved =  charactersDictionary.Remove (characterName);

			if (character.IsPlayer) 
			{
				BasePlayerCharacterController player = character as BasePlayerCharacterController;
				int index = _playersList.IndexOf (player);
				_playersList.RemoveAt (index);
			}

			if (!valueRemoved) 
			{
				// Exeption
			}
		}

		public BaseCharacterController GetCharacterInstance(string characterName)
		{
			BaseCharacterController character = null;
			bool containsValue = charactersDictionary.TryGetValue (characterName, out character);

			if (!containsValue) 
			{
				//	exeption
			}

			return character;
		}

		public BasePlayerCharacterController GetPlayerCharacterInstance(int index)
		{
			if (index < 0 || index > _playersList.Count - 1)
				return null;
			
			return _playersList [index];
		}
	}
}
