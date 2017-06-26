using UnityEngine;

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class ChainProcessor : MonoBehaviour 
	{
		private const string Link_Container_Object_Name = "LinkContainerObject";
		#if UNITY_EDITOR

		[SerializeField]
		private BaseChainLinkFactory _linksFactory = null;
		public BaseChainLinkFactory LinksFactory { get { return this._linksFactory; } }

		#endif

		[SerializeField]
		private List<ChainLink> _linkList = new List<ChainLink>();
		public List<ChainLink> LinkList 
		{
			get { return this._linkList; }
		}

		[SerializeField]
		private GameObject _linkContainerObject = null;
		public GameObject LinkContainerObject 
		{
			get 
			{ 
				if (this._linkContainerObject == null) 
				{
					_linkContainerObject = new GameObject ();
					_linkContainerObject.transform.SetParent (this.transform);
					_linkContainerObject.transform.localPosition = Vector3.zero;
					_linkContainerObject.transform.localRotation = Quaternion.identity;
					_linkContainerObject.transform.localScale = Vector3.one;
					_linkContainerObject.name = Link_Container_Object_Name;
				}

				return this._linkContainerObject; 
			}
			set { _linkContainerObject = value; }
		}
	}
}