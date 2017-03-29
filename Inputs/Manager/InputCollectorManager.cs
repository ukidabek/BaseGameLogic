using UnityEngine;

using System.Collections;
using System.Collections.Generic;

namespace BaseGameLogic.Inputs
{
	public class InputCollectorManager : MonoBehaviour 
	{
		[SerializeField]
		private List<InputCollector> _inputCollectors = new List<InputCollector>();

		private Dictionary<string, InputCollector> _inputCollectorsDictionary = new Dictionary<string, InputCollector>();

		public void Awake()
		{
			for (int i = 0; i < _inputCollectors.Count; i++) 
			{
				_inputCollectorsDictionary.Add (i.ToString (), _inputCollectors [i]);
			}
		}
	}
}