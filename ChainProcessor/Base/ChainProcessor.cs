using UnityEngine;

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	public class ChainProcessor : MonoBehaviour 
	{
		private const string Link_Container_Object_Name = "LinkContainerObject";

		[SerializeField, HideInInspector]
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

		#if UNITY_EDITOR

		[SerializeField, Space(10)]
		private BaseChainLinkFactory _linksFactory = null;
		public BaseChainLinkFactory LinksFactory { get { return this._linksFactory; } }

		public Action Reprint = null;

		#endif

		[SerializeField, HideInInspector]
		private List<ChainLink> _linkList = new List<ChainLink>();
		public List<ChainLink> LinkList 
		{
			get { return this._linkList; }
		}

		[SerializeField, HideInInspector]
		private List<ChainInput> _inputs = new List<ChainInput>();
		public List<ChainInput> Inputs 
		{
			get { return this._inputs; }
		}

		[SerializeField, HideInInspector]
		private List<ChainOutput> _outputs = new List<ChainOutput>();
		public List<ChainOutput> Outputs 
		{
			get { return this._outputs; }
		}

		public void Process()
		{
			#if UNITY_EDITOR
			if(Reprint != null)
			{
				Reprint();
			}
			#endif

			for (int i = 0; i < _inputs.Count; i++) 
			{
				_inputs [i].Prosess ();
			}
		}
	}
}