using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Character
{
	public class CharacterRegister : MonoBehaviour 
	{
		private Dictionary<string, BaseCharacterController> charactersDictionary = new Dictionary<string, BaseCharacterController>();

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
			}
		}

		public void UnregisterCharacter(BaseCharacterController character)
		{
			string characterName = character.name;
			bool valueRemoved =  charactersDictionary.Remove (characterName);

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
	}
}
