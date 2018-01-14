using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Singleton;

namespace BaseGameLogic.Character
{
	public class CharacterRegister : Singleton<CharacterRegister>
    {
        #if UNITY_EDITOR
        /// <summary>
        /// List of characters. In editor only. 
        /// </summary>
        [SerializeField]
        private List<BaseCharacterController> _charactersList = new List<BaseCharacterController>();
        #endif
        /// <summary>
        /// Characters dictionary.
        /// </summary>
        private Dictionary<string, BaseCharacterController> _charactersDictionary = new Dictionary<string, BaseCharacterController>();

        #if UNITY_EDITOR
        /// <summary>
        /// List of players. In editor only. 
        /// </summary>
        [SerializeField]
		private List<BasePlayerCharacterController> _playersList = new List<BasePlayerCharacterController>();
        #endif
        /// <summary>
        /// Players dictionary.
        /// </summary>
        private Dictionary<int, BasePlayerCharacterController> _playerDictionary = new Dictionary<int, BasePlayerCharacterController>();

        /// <summary>
        /// Register character
        /// </summary>
        /// <param name="character">Character instance. </param>
        public void RegisterCharacter(BaseCharacterController character)
		{
			bool containsValue = _charactersDictionary.ContainsValue (character);
			if (containsValue) 
			{
				//	Exception
			} 
			else 
			{
				string characterName = character.name;

                #if UNITY_EDITOR
                _charactersList.Add(character);
                #endif

                _charactersDictionary.Add (characterName, character);
                RegisterPlayer(character);
            }
		}

        /// <summary>
        /// Register character as player if character is player.
        /// </summary>
        /// <param name="character">Character instance.</param>
        private void RegisterPlayer(BaseCharacterController character)
        {
            if (character.IsPlayer)
            {
                BasePlayerCharacterController player = character as BasePlayerCharacterController;
                bool containsValue = _playerDictionary.ContainsValue(player);
                if (containsValue)
                {
                    //	Exception
                }
                else
                {
                    _playerDictionary.Add(player.PlayerNumber, player);
                    
                    #if UNITY_EDITOR
                    _playersList.Add(player);
                    _playersList.Sort();
                    #endif
                }
            }
        }

        /// <summary>
        /// Unregister character
        /// </summary>
        /// <param name="character">Character instance.</param>
        public void UnregisterCharacter(BaseCharacterController character)
		{
			string characterName = character.name;
			bool valueRemoved =  _charactersDictionary.Remove (characterName);

            UnregisterPlayer(character);

			if (!valueRemoved) 
			{
				// Exception
			}
		}

        /// <summary>
        /// Unregister character from players if character is player.
        /// </summary>
        /// <param name="character">Character instance.</param>
        public void UnregisterPlayer(BaseCharacterController character)
        {
            if (character.IsPlayer)
            {
                BasePlayerCharacterController player = character as BasePlayerCharacterController;
                int playerNumber = player.PlayerNumber;
                bool valueRemoved = _playerDictionary.Remove(playerNumber);
                if(valueRemoved)
                {
                    #if UNITY_EDITOR
                    int index = _playersList.IndexOf(player);
                    _playersList.RemoveAt(index);
                    #endif
                }
            }
        }

        /// <summary>
        /// Return character instance by it's name.
        /// </summary>
        /// <param name="characterName">Character name.</param>
        /// <returns>BaseCharacterController instance.</returns>
        public BaseCharacterController GetCharacterInstance(string characterName)
		{
			BaseCharacterController character = null;
			bool containsValue = _charactersDictionary.TryGetValue (characterName, out character);
			if (!containsValue) 
			{
				//	Exception
			}

			return character;
		}

        /// <summary>
        /// Return player instance by it's number.
        /// </summary>
        /// <param name="index"></param>
        /// <returns>BasePlayerCharacterController instance.</returns>
		public BasePlayerCharacterController GetPlayerCharacterInstance(int index)
		{
            BasePlayerCharacterController player = null;
            if(_playerDictionary.TryGetValue(index, out player))
            {
                return player;
            }

            return null;
		}
	}
}
