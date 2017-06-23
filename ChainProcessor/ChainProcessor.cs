using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class ChainProcessor : MonoBehaviour 
	{
		[SerializeField]
		private List<ChainLink> _linkList = new List<ChainLink>();
		public List<ChainLink> LinkList 
		{
			get { return this._linkList; }
		}
	}
}