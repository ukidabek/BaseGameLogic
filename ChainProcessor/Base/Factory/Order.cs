using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class Order
	{
		private Vector3 _position = Vector3.zero;
		public Vector3 Position 
		{
			get { return this._position; }
			set { _position = value; }
		}

		private string _type = string.Empty;
		public string Type { get { return this._type; } }

		private GameObject _linkContainerObject = null;
		public GameObject LinkContainerObject 
		{
			get { return this._linkContainerObject; }
		}

		public Order (string _type, GameObject _linkContainerObject)
		{
			this._type = _type;
			this._linkContainerObject = _linkContainerObject;
		}
		
	}
}