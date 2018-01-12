using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Singleton;

namespace BaseGameLogic.Inputs
{
	public abstract class InputCollectorManager : Singleton<InputCollectorManager>
    {
        [SerializeField]
        [Tooltip("List of input collectors active in game.")]
        private InputCollector [] _inputCollectors = null;

        /// <summary>
        /// 
        /// </summary>
		private Dictionary<int, InputCollector> _inputCollectorsDictionary = new Dictionary<int, InputCollector>();

		protected override void Awake()
		{
            base.Awake();

            // Collecting InputCollector instances. 
            _inputCollectors = GetComponentsInChildren<InputCollector>();

            // Creating dictionary from collected InputCollectors.
            for (int i = 0; i < _inputCollectors.Length; i++) 
			{
				_inputCollectorsDictionary.Add ( _inputCollectors [i].PlayerNumber, _inputCollectors [i]);
			}
		}

        /// <summary>
        /// Returns InputCollector by it's PlayerNumber. 
        /// If there are only one InputCollector instance method will return that one.
        /// </summary>
        /// <param name="playerNumber">PlayerNumber</param>
        /// <returns>InputCollector instance.</returns>
		public InputCollector GetInputCollector(int playerNumber = 0)
		{
			InputCollector _inputCollector = null;

            if(_inputCollectorsDictionary.Count == 1)
            {
                _inputCollectorsDictionary.TryGetValue(0, out _inputCollector);
            }
            else
            {
                _inputCollectorsDictionary.TryGetValue (playerNumber, out _inputCollector);
            }

			 return _inputCollector;
		}
	}
}