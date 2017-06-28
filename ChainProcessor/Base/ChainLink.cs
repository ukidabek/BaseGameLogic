using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

using System;
using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.Management;

namespace BaseGameLogic.ChainProcessing
{
	[Serializable]
	public abstract class ChainLink : MonoBehaviour
	{
		public abstract string Name { get; }

		public ChainData OutData = null;

		#if UNITY_EDITOR

		[SerializeField]
		protected Rect _linkRect = new Rect ();
		public Rect LinkRect 
		{
			get { return this._linkRect; }
			set { _linkRect = value; }
		}
			
		public Vector2 OutputHook()
		{
				Vector2 hook = new Vector2 (
					               _linkRect.position.x + _linkRect.size.x,
					               _linkRect.position.y + (_linkRect.size.y / 2));
			
				return hook;
		}

		public Vector2 GetInputHook(int index)
		{
			float deltaHeight = _linkRect.size.y / (InputsCount * 2);

			Vector2 hook = new Vector2 (
				_linkRect.position.x,
				_linkRect.position.y + (deltaHeight + (deltaHeight * index * 2)));

			return hook;
		}

		#endif

		protected abstract int InputsCount { get ; }

		[SerializeField]
		protected ChainLink[] _inputs = null; 
		public ChainLink [] Inputs 
		{
			get 
			{ 
				if (_inputs == null) 
				{
					_inputs = new ChainLink [InputsCount];
				}

				return _inputs; 
			}
		}


		[SerializeField]
		public List<ChainLink> _outputs = null;
		public List<ChainLink> Outputs 
		{ 
			get 
			{
				if (_outputs == null) 
				{
					_outputs = new List<ChainLink>();
				}
				return _outputs; 
			} 
		}

		public abstract void Prosess();

		#if UNITY_EDITOR

		public virtual void DrawNodeWindow(int id) 
		{
			GUI.DragWindow();
		}

		#endif

		public ChainLink (Vector2 position)
		{
			this._linkRect.x = position.x;
			this._linkRect.y = position.y;
		}
		
	}
}