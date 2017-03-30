using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Inputs
{
	public class InputCollectorManager : MonoBehaviour 
	{
		[SerializeField]
		private List<InputCollector> _inputCollectors = new List<InputCollector>();

		private Dictionary<int, InputCollector> _inputCollectorsDictionary = new Dictionary<int, InputCollector>();

		public void Awake()
		{
			for (int i = 0; i < _inputCollectors.Count; i++) 
			{
				_inputCollectorsDictionary.Add (
					_inputCollectors [i].PlayerNumber, 
					_inputCollectors [i]);
			}
		}

		public InputCollector GetInputCollector(int id)
		{
			InputCollector _inputCollector = null;

			_inputCollectorsDictionary.TryGetValue (id, out _inputCollector);

			 return _inputCollector;
		}
	}
}